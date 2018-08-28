from django.shortcuts import render
from . models import *
from . serializers import *
from rest_framework.views import APIView
from rest_framework.renderers import TemplateHTMLRenderer
from rest_framework.response import Response
from django.http import HttpResponseRedirect
from rest_framework import status


# def index(request):
#     latest_posts = Post.objects.order_by('-created')[:5]
#     data_list = []
#     likeS = LikeSerializer()
#     for post in latest_posts:
#         comments = Comment.objects.filter(post=post).order_by('-created')[:5]
#         likes = len(likeS.get_post_likes(post.id))
#         data_list.append((post, likes, comments))

#     context = {'data': data_list}
#     return render(request, 'reddit_app/index.html', context)


class Index(APIView):
    def get(self, request):
        posts = Post.objects.all()
        serializer = IndexSerializer(posts, many=True)
        print(serializer.data)
        return Response(serializer.data, status=status.HTTP_200_OK)


class LogIn(APIView):
    
    def get(self, request):
        serializer = LogInSerializer()
        return Response(serializer.data)

    def post(self, request):
        serializer = LogInSerializer()
        data = serializer.validate(request.data)
        
        username = data['username']
        token = data['token']

        return Response({'username': username, 'token': token}, status=status.HTTP_200_OK)


class SignUp(APIView):

    def get(self, request):
        serializer = SignupSerializer()
        return Response(serializer.data)
    
    def post(self, request):
        try:
            serializer = SignupSerializer()
            post = request.POST.copy()
            try:
                photo = request.FILES['avatar']
            except:
                photo = None
            serializer.create(validated_data=dict(post), photo=photo)
            return Response(serializer.data, status=status.HTTP_201_CREATED)
        except:
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)


class MakePost(APIView):

    def get(self, request):
        serializer = MakePostSerializer()
        return Response(serializer.data)

    def post(self, request):
        try:
            if request.user.is_authenticated:
                user = request.user
                serializer = MakePostSerializer()
                post = request.POST.copy()
                try:
                    photo = request.FILES['photo']
                except:
                    photo = None

                serializer.create(validated_data=dict(post), user=user, foto=photo)
                return Response(serializer.data)
            else:
                return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

        except:
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)


class PostView(APIView):

    def get(self, request, pid):
        serializer = PostSerializer()
        return Response(serializer.data)

    def post(self, request, pid):
        if request.user.is_authenticated:
            serializer = CommSerializer()
            user = request.user
            post = Post.objects.get(id=pid)
            vd = request.POST.copy()
            serializer.create(validated_data=dict(vd), user=user, post=post)
            return Response(serializer.data)
        return HttpResponseRedirect("/login")

    def put(self, request, pid):
        if request.user.is_authenticated:
            serializer = LikeSerializer()
            user = request.user
            serializer.create(uid=user.id, pid=pid)
            return Response(serializer.data)
        return HttpResponseRedirect("/login/")


class ProfileView(APIView):

    def get(self, request):
        user = request.user
        serializer = CustomUserSerializer(user)
        return Response(serializer.data)
    
    def post(self, request):
        if request.user.is_authenticated:
            serializer = SignupSerializer()
            vd = request.POST.copy()
            print(vd)
            serializer.update(validated_data=vd, uid=request.user.id)
            return HttpResponseRedirect("/profile")
