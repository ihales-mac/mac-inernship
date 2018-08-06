from django.contrib.auth import get_user_model
from rest_framework.generics import CreateAPIView

from post_app.serializers import UserCreateSerializer, Profile, CommentCreateSerializer, Comment

User = get_user_model()

class UserCreateAPIView(CreateAPIView):
    serializer_class = UserCreateSerializer
    queryset = Profile.objects.all()

