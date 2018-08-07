

from django.contrib.auth import get_user_model
from rest_framework.authentication import SessionAuthentication
from rest_framework.authtoken.models import Token
from rest_framework.authtoken.views import ObtainAuthToken
from rest_framework.decorators import permission_classes
from rest_framework.generics import CreateAPIView, ListAPIView
from rest_framework.permissions import AllowAny, IsAuthenticated
from rest_framework.response import Response
from rest_framework.status import HTTP_200_OK, HTTP_400_BAD_REQUEST
from rest_framework.views import APIView
from django.contrib.auth.models import Permission

from post_app.models import Poster
from post_app.serializers import UserCreateSerializer, Profile, CommentCreateSerializer, Comment, UserLoginSerializer

User = get_user_model()
@permission_classes((AllowAny, ))
class UserCreateAPIView(CreateAPIView):
    serializer_class = UserCreateSerializer
    queryset = Profile.objects.all()

class UserLoginAPIView(APIView):
    permission_classes = [AllowAny]
    serializer_class = UserLoginSerializer

    def post(self, request, *args, **kwargs):
        data = request.data
        serializer = UserLoginSerializer(data = data)

        if serializer.is_valid():
            new_data = serializer.data

            user = Poster.objects.get(username=new_data["username"])
            token, created = Token.objects.get_or_create(user=user)
            return Response({
                'token': token.key,
                'user_id': user.pk})

        return Response(serializer.errors, status = HTTP_400_BAD_REQUEST)


class UserLogoutAPIView(APIView):
    permission_classes = [IsAuthenticated]

    def get(self, request, *args, **kwargs):
        if 'user' in kwargs:
            user = kwargs['user']
        else:
            user = request.user.username

        try:
            user = Poster.objects.get(username=user)
        except:
            return Response(status=HTTP_400_BAD_REQUEST)
        Token.objects.filter(user=user).delete()
        return Response(status = HTTP_200_OK)
