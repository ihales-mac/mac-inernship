from django.contrib.auth import login, logout
from django.http import Http404
from django.shortcuts import render
from rest_framework.exceptions import ValidationError
from rest_framework.permissions import IsAuthenticated, AllowAny
from rest_framework.renderers import JSONRenderer
from rest_framework.response import Response
from rest_framework.status import HTTP_200_OK, HTTP_400_BAD_REQUEST
from rest_framework.views import APIView
from rest_framework.authentication import SessionAuthentication, BasicAuthentication, TokenAuthentication
from rest_framework.authtoken.models import Token
from redditapp.models import Post, CustomUser, UserDetails
from redditapp.serializers import PostListSerializer, PostDetailSerializer, UserCreateSerializer, \
    UserLoginSerializer, UserProfileSerializer, CreatePostSerializer, GiveCommentSerializer, GiveLikeSerializer
from rest_framework import generics
import pprint
import json


# version 1
# class PostListView(APIView):
#
#     def get(self,request):
#         posts = Post.objects.all()
#         serializer = PostListSerializer(posts,many=True)
#         return Response(serializer.data)

# version 2
class PostListView(generics.ListCreateAPIView):
    queryset = Post.objects.all()
    serializer_class = PostListSerializer

    # Moved to settings
    # authentication_classes = [TokenAuthentication, SessionAuthentication, BasicAuthentication]
    # permission_classes = [IsAuthenticated, ]

    def list(self, request):
        queryset = self.get_queryset()
        serializer = PostListSerializer(queryset, many=True)
        return Response(serializer.data)


class PostDetailView(APIView):

    def get_object(self, pk):
        try:
            return Post.objects.get(pk=pk)
        except Post.DoesNotExist:
            raise Http404

    def get(self, request, pk, format=None):
        post = self.get_object(pk)
        serializer = PostDetailSerializer(post)

        # For debuggin
        # pp = pprint.PrettyPrinter(indent=4)
        # p2 = JSONRenderer().render(serializer.data)
        # pp.pprint(json.loads(p2))

        return Response(serializer.data)


class UserProfileView(APIView):
    def get_object(self, username):
        try:
            return CustomUser.objects.get(username=username)
        except CustomUser.DoesNotExist:
            raise Http404

    def get(self, request, username, format=None):
        custom_user = self.get_object(username)
        serializer = UserProfileSerializer(custom_user)

        # For debugin
        # pp = pprint.PrettyPrinter(indent=4)
        # p2 = JSONRenderer().render(serializer.data)
        # pp.pprint(json.loads(p2))

        return Response(serializer.data)


class UserCreateView(APIView):
    # version 1
    # serializer_class = UserCreateSerializer
    # queryset = CustomUser.objects.all()

    def post(self, request):
        serializer = UserCreateSerializer(data=request.data)
        if serializer.is_valid(raise_exception=True):
            serializer.save()
            return Response(status=HTTP_200_OK)
        return Response(serializer.errors, status=HTTP_400_BAD_REQUEST)


class UserLoginView(APIView):
    permission_classes = [AllowAny, ]

    def post(self, request):
        serializer = UserLoginSerializer(data=request.data)
        if serializer.is_valid():
            username = serializer.validated_data['username']
            token = serializer.validated_data['token']
            return Response({'username': username, "token": token}, status=HTTP_200_OK)
        return Response(serializer.errors, status=HTTP_400_BAD_REQUEST)


# Version 2
# class LoginView(APIView):
#
#     def post(self, request):
#         serialzier = LoginSerializer(data=request.data)
#         serialzier.is_valid(raise_exception=True)
#         user = serialzier.validated_data['user']
#         login(request, user)
#         token, created = Token.objects.get_or_create()
#         return Response({"token": token.key}, status=200)
#
#
# class LogoutView(APIView):
#     authentication_classes = (TokenAuthentication,)
#
#     def post(self, request):
#         logout(request)
#         return Response(status=204)

class CreatePostView(APIView):

    def post(self, request):
        serializer = CreatePostSerializer(data=request.data)
        if serializer.is_valid():
            serializer.save(owner=request.user)
            return Response(serializer.data, status=HTTP_200_OK)
        return Response(serializer.errors, status=HTTP_400_BAD_REQUEST)


class GiveCommentView(APIView):
    def get_object(self, pk):
        try:
            return Post.objects.get(pk=pk)
        except Post.DoesNotExist:
            raise Http404

    def post(self, request, pk, format=None):
        post = self.get_object(pk)
        serializer = GiveCommentSerializer(data=request.data)
        if serializer.is_valid():
            serializer.save(owner=request.user, post=post)
            return Response(serializer.data, status=HTTP_200_OK)
        return Response(serializer.errors, status=HTTP_400_BAD_REQUEST)


class GiveLikeView(APIView):
    def get_object(self, pk):
        try:
            return Post.objects.get(pk=pk)
        except Post.DoesNotExist:
            raise Http404

    def post(self, request, pk, format=None):
        post = self.get_object(pk)
        serializer = GiveLikeSerializer(data=request.data)
        if serializer.is_valid(raise_exception=True):
            try:
                serializer.save(owner=request.user, post=post)
            except Exception as e:
                raise ValidationError({"status_code": 405, "details": e})
            return Response(serializer.data, status=HTTP_200_OK)
        return Response(serializer.errors, status=HTTP_400_BAD_REQUEST)
