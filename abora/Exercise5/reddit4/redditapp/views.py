from django.contrib.auth.decorators import login_required
from django.http import Http404
from django.shortcuts import render, redirect
from django.views.decorators.csrf import csrf_exempt
from rest_framework.response import Response
from rest_framework.views import APIView
from rest_framework.renderers import TemplateHTMLRenderer
from rest_framework import generics

from redditapp.models import Post, CustomUser, Comment, UserDetails
from redditapp.serializers import CustomUserSerializerPost, PostSerializer, \
    CreatePostSerializer, GiveCommentSerializer, GiveLikeSerializer, ProfileSerializer
from rest_framework.renderers import JSONRenderer
import pprint
import json


class SignUp(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'redditapp/sign_up.html'

    def get(self, request):
        serializer = CustomUserSerializerPost()
        return Response({'serializer': serializer})

    def post(self, request):
        serializer = CustomUserSerializerPost(data=request.data)
        if not serializer.is_valid():
            return Response({'serializer': serializer})
        serializer.save()
        return redirect('reddit_home')


class PostListView(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'redditapp/index.html'

    def get(self, request):
        posts = Post.objects.all()
        query = request.GET.get('q')
        if query:
            posts = Post.objects.filter(title__icontains=query)
        serializer = PostSerializer(posts, many=True)
        pp = pprint.PrettyPrinter(indent=4)

        p2 = JSONRenderer().render(serializer.data)
        pp.pprint(json.loads(p2))
        return Response({'posts': serializer.data})


class CreatePostView(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'redditapp/create_new_post.html'

    def get(self, request):
        serializer = CreatePostSerializer()
        return Response({'serializer': serializer})

    def post(self, request):
        serializer = CreatePostSerializer(data=request.data)
        if not serializer.is_valid():
            return Response({'serializer': serializer.data})
        user = CustomUser.objects.filter(username=request.user.username).first()
        serializer.save(owner=user)
        return redirect('reddit_home')


class PostDetailsView(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'redditapp/post_details.html'

    def get_object(self, pk):
        try:
            return Post.objects.get(pk=pk)
        except Post.DoesNotExist:
            raise Http404

    def get(self, request, pk, format=None):
        post = self.get_object(pk)
        serializer = PostSerializer(post)
        pp = pprint.PrettyPrinter(indent=4)
        p2 = JSONRenderer().render(serializer.data)
        pp.pprint(json.loads(p2))
        serializerComment = GiveCommentSerializer()

        return Response({'post': serializer.data, 'comment_serializer': serializerComment})
    #
    # def post(self, request, format=None):
    #     serializer = SnippetSerializer(data=request.data)
    #     if serializer.is_valid():
    #         serializer.save()
    #         return Response(serializer.data, status=status.HTTP_201_CREATED)
    #     return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
    #


class GiveCommentView(APIView):
    def get_object(self, pk):
        try:
            return Post.objects.get(pk=pk)
        except Post.DoesNotExist:
            raise Http404

    def post(self, request, pk, format=None):
        post = self.get_object(pk)
        user = CustomUser.objects.filter(username=request.user.username).first()
        serializer = GiveCommentSerializer(data=request.data)
        if not serializer.is_valid():
            return Response({'serializer': serializer.data})
        serializer.save(owner=user, post=post)
        return redirect('/post-detail/{}/'.format(pk))


class GiveLikeView(APIView):
    def get_object(self, pk):
        try:
            return Post.objects.get(pk=pk)
        except Post.DoesNotExist:
            raise Http404

    def post(self, request, pk, format=None):
        post = self.get_object(pk)
        user = CustomUser.objects.filter(username=request.user.username).first()
        serializer = GiveLikeSerializer(data=request.data)
        if not serializer.is_valid():
            return Response({'serializer': serializer.data})
        serializer.save(owner=user, post=post)
        return redirect('/post-detail/{}/'.format(pk))


class ProfileView(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'redditapp/profile.html'

    def get_object(self, request):
        try:
            user = CustomUser.objects.filter(username=request.user.username).first()
            user_details = UserDetails.objects.filter(user=user).first()
            return user_details
        except UserDetails.DoesNotExist:
            raise Http404

    def get(self, request):
        user_details = self.get_object(request)
        serializer = ProfileSerializer(user_details)

        pp = pprint.PrettyPrinter(indent=4)
        p2 = JSONRenderer().render(serializer.data)
        pp.pprint(json.loads(p2))

        return Response({'serializer': serializer.data})
