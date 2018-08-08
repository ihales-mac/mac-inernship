
from django.contrib.auth import get_user_model
from rest_framework import authentication, exceptions
from rest_framework.generics import *
from rest_framework.response import Response
from rest_framework import status
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


class UserDetailsList(RetrieveAPIView):
    queryset = Profile.objects.all()
    serializer_class = ProfileSerializer


class CommentCreateAPIView(ListCreateAPIView):
    queryset = Comment.objects.all()
    serializer_class = CommentCreateSerializer

    def perform_create(self, serializer):
        post = self.kwargs['post']
        name = self.request.user.username
        user = Profile.objects.get(user__username= name)
        serializer.save(user=user, post_id=post)


class LikeCreateAPIView(APIView):
    #queryset = Like.objects.all()
    #serializer_class = LikeCreateSerializer

    def post(self, request, post):

        id = request.user.id
        user = Poster.objects.get(id=id)
        post = Post.objects.get(id = post)
        try:
            Like.objects.get(user=user, post=post)
            return Response(data= {'error': 'already liked'} ,status = status.HTTP_400_BAD_REQUEST)
        except:
            Like.objects.create(user=user, post=post)
            post.nr_likes = Like.objects.filter(post=post).count()
            post.save()
        return Response(status = status.HTTP_200_OK)

