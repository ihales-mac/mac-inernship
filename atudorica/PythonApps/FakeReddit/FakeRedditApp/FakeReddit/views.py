from django.shortcuts import render
from FakeReddit.models import *
from FakeReddit.serializers import *
from rest_framework.views import APIView
from rest_framework.renderers import TemplateHTMLRenderer
from rest_framework.response import Response

from django.http import HttpResponseRedirect, HttpResponse


class index(APIView):
    renderer_classes = [TemplateHTMLRenderer]

    def get(self, request):
        latest_posts = Post.objects.order_by('-created')[:5]
        data_list = []
        for post in latest_posts:
            comments = Comment.objects.filter(post=post).order_by('-created')[:5]
            likes = Like.objects.filter(post=post).count
            data_list.append((post, likes, comments))
        context = {'data': data_list}
        return render(request, 'index.html', context)


class editprofile(APIView):
    renderer_classes = [TemplateHTMLRenderer]

    def get(self, request):
        serializer = EditProfileSerializer(request.user)
        return Response({'serializer': serializer})

    def post(self, request):
        serializer = EditProfileSerializer()
        post = request.POST.copy()
        try:
            photo = request.FILES['avatar']
        except:
            photo = None
        print(str(photo))
        serializer.edit(validated_data=dict(post), photo=photo, id=request.user.id)
        return HttpResponseRedirect("/")


class signup(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'signup.html'

    def get(self, request):
        serializer = ProfileSerializer
        return Response({'serializer': serializer})

    def post(self, request):
        serializer = ProfileSerializer()
        post = request.POST.copy()
        try:
            photo = request.FILES['avatar']
        except:
            photo = None
        print(str(photo))
        serializer.create(validated_data=dict(post), photo=photo)
        return HttpResponseRedirect("/")


class makepost(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'makepost.html'

    def get(self, request):
        if request.user.is_authenticated:
            serializer = MakePostSerializer
            return Response({'serializer': serializer})
        return HttpResponseRedirect("/")

    def post(self, request):
        if request.user.is_authenticated:
            user = request.user
            serializer = MakePostSerializer()
            post = request.POST.copy()
            photo = request.FILES['photo']
            serializer.create(validated_data=dict(post), user=user, photo=photo)
        return HttpResponseRedirect("/")


class viewpost(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'viewpost.html'

    def get(self, request, pid):
        serializer = PostSerializer()
        cs = CommSerializer()
        post, comments = serializer.get_post(pid)
        likes = Like.objects.filter(post=post).count
            
        context = {'likes': likes, 'post': post, 'comments': comments,  'serializer': cs}
        return render(request, 'viewpost.html', context)

    def delete(self, request, pid):
        if request.user.is_authenticated:
            current_post = Post.objects.get(pk=pid, user=request.user)
            if current_post:
                current_post.delete()
        return HttpResponseRedirect("/")

    def post(self, request, pid):
        if request.user.is_authenticated:
            serializer = CommSerializer()
            user = request.user
            post = Post.objects.get(id=pid)
            vd = request.POST.copy()
            serializer.create(validated_data=dict(vd), user=user, post=post)
            return HttpResponseRedirect("/viewpost/" + pid)
    
    def put(self, request, pid):
        if request.user.is_authenticated:
            post = request.POST.copy()
            print(pid)
            serializer = LikeSerializer()
            user = request.user
            post = Post.objects.get(id=pid)
            try:
                serializer.create(user=user, post=post)
            except:
                print("This user already liked this post once.")
            return HttpResponseRedirect("/viewpost/" + pid+'/')
        return HttpResponseRedirect("login/")
