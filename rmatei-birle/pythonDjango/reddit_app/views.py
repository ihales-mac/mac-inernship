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
    likeS = LikeSerializer()
    for post in latest_posts:
        comments = Comment.objects.filter(post=post).order_by('-created')[:5]
        likes = len(likeS.get_post_likes(post.id))
        data_list.append((post, likes, comments))

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
    template_name = 'reddit_app/post.html'

    def get(self, request, pid):
        serializer = PostSerializer()
        serializerComm = CommSerializer()
        post, comments = serializer.get_post(pid)
        likeS = LikeSerializer()
        likes = len(likeS.get_post_likes(post.id))
        context = {
            'post': post,
            'likes': likes,
            'comments': comments,
            'serializer': serializerComm,
            }
        return render(request, 'reddit_app/post.html', context)

    def delete(self, request, pid):
        if request.user.is_authenticated:
            current_post = Post.objects.get(pk=pid, user=request.user)
            if current_post:
                current_post.delete()
                return HttpResponseRedirect("/")
        return HttpResponseRedirect("/login")

    def post(self, request, pid):
        if request.user.is_authenticated:
            serializer = CommSerializer()
            user = request.user
            post = Post.objects.get(id=pid)
            vd = request.POST.copy()
            serializer.create(validated_data=dict(vd), user=user, post=post)
            return HttpResponseRedirect("/post/" + pid)
        return HttpResponseRedirect("/login")

    def put(self, request, pid):
        if request.user.is_authenticated:
            serializer = LikeSerializer()
            user = request.user
            serializer.create(uid=user.id, pid=pid)
            return HttpResponseRedirect("/post/{0}/".format(pid))
        return HttpResponseRedirect("/login/")


class ProfileView(APIView):
    render_classes = [TemplateHTMLRenderer]
    template_name = 'reddit_app/profile.html'

    def get(self, request):
        user = request.user
        serializer = CustomUserSerializer(user)
        likeS = LikeSerializer()
        likes = likeS.get_user_likes(user.id)
        context = {
            'serializer': serializer.data,
            'likes': likes
            }
        return render(request, 'reddit_app/profile.html', context)
    
    def post(self, request):
        if request.user.is_authenticated:
            serializer = SignupSerializer()
            vd = request.POST.copy()
            print(vd)
            serializer.update(validated_data=vd, uid=request.user.id)
            return HttpResponseRedirect("/profile")
