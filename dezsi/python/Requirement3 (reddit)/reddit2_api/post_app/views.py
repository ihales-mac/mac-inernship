from django.contrib.auth import get_user_model
from rest_framework.generics import *
from rest_framework.views import APIView

from post_app.serializers import *


class PostCreateAPIView(ListCreateAPIView):
    queryset = Post.objects.all()
    serializer_class = PostCreateUpdateSerializer

    def perform_create(self, serializer):
        serializer.save(user= self.request.user)


class PostListViewAPI(ListAPIView):
    queryset = Post.objects.all()
    serializer_class = PostSerializer

    def get_queryset(self):
        #do stuff with queryset
        return self.queryset.all()


class PostDetailAPIView(RetrieveAPIView):
    queryset = Post.objects.all()
    serializer_class = PostSerializer


class PostUpdateAPIView(RetrieveUpdateAPIView):
    queryset = Post.objects.all()
    serializer_class = PostCreateUpdateSerializer

    '''
    def perform_create(self, serializer):
        serializer.save(user= self.request.user)
  
    '''


class PostDeleteAPIView(DestroyAPIView):
    queryset = Post.objects.all()
    serializer_class = PostSerializer


class Login(APIView):
    pass


class UserDetailsList(RetrieveAPIView):
    queryset = Profile.objects.all()
    serializer_class = ProfileSerializer


class CommentCreateAPIView(ListCreateAPIView):
    queryset = Comment.objects.all()
    serializer_class = CommentCreateSerializer

    def perform_create(self, serializer):
        serializer.save(user=self.request.user)
