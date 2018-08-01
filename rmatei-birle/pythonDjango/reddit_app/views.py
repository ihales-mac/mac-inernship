from django.shortcuts import render
from . models import *
from . serializers import *
from rest_framework.views import APIView
from rest_framework.renderers import TemplateHTMLRenderer
from rest_framework.response import Response
from django.http import HttpResponseRedirect


def index(request):
    latest_posts = Post.objects.order_by('-created')[:5]
    data_list = []
    for post in latest_posts:
        comments = Comment.objects.filter(post=post).order_by('-created')[:5]
        data_list.append((post, comments))

    context = {'data': data_list}
    return render(request, 'reddit_app/index.html', context)


class SignUp(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'reddit_app/signup.html'

    def get(self, request):
        serializer = SignupSerializer()
        return Response({'serializer': serializer})
    
    def post(self, request):
        serializer = SignupSerializer()
        post = request.POST.copy()
        try:
            photo = request.FILES['avatar']
        except:
            photo = None
        serializer.create(validated_data=dict(post), photo=photo)
        return HttpResponseRedirect("/")


class MakePost(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'reddit_app/makepost.html'

    def get(self, request):
        serializer = MakePostSerializer()
        return Response({'serializer': serializer})

    def post(self, request):
        if request.user.is_authenticated:
            user = request.user
            serializer = MakePostSerializer()
            post = request.POST.copy()
            try:
                photo = request.FILES['photo']
            except:
                photo = None

            serializer.create(validated_data=dict(post), user=user, foto=photo)

        return HttpResponseRedirect("/")


class PostView(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'reddit/post.html'

    def get(self, request, pid):
        serializer = PostSerializer()
        serializerComm = CommSerializer()
        post, comments = serializer.get_post(pid)
        context = {'post': post, 'comments': comments,  'serializer': serializerComm}
        return render(request, 'reddit_app/post.html', context)

    def delete(self, request, pid):
        if request.user.is_authenticated:
            current_post = Post.objects.get(pk=pid, user=request.user)
            if current_post:
                # Post.objects.delete(current_post)
                current_post.delete()
        return HttpResponseRedirect("/")

    def post(self, request, pid):
        if request.user.is_authenticated:
            serializer = CommSerializer()
            user = request.user
            post = Post.objects.get(id=pid)
            vd = request.POST.copy()
            serializer.create(validated_data=dict(vd), user=user, post=post)
            return HttpResponseRedirect("/post/" + pid)
