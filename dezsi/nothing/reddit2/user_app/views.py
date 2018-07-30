from pprint import pprint

from django.contrib.auth import authenticate, login, logout
from django.contrib.auth.decorators import login_required
from django.db import connection
from django.http import HttpResponseRedirect
from django.shortcuts import render, redirect, get_object_or_404

# Create your views here.
from django.shortcuts import render

from rest_framework.renderers import TemplateHTMLRenderer
from rest_framework.response import Response
from rest_framework.views import APIView

from post_app.models import *
from post_app.serializers import *
from user_app.serializers import ProfileSerializerFlat, ProfileSerializerStruct


class DetailsList(APIView):

    renderer_classes = [TemplateHTMLRenderer]
    template_name = "user_app/details.html"

    def get(self, request):
        usern = request.user.username
        profiles = Profile.objects.filter(user__username = usern)
        serializer1 = ProfileSerializerStruct(profiles, many=True)
        #inner join
        posters = Poster.objects.filter(id=serializer1.data[-1]["user"])
        serializer2 = PosterSerializer(posters, many=True)
        return Response({'details': serializer1.data, 'poster': serializer2.data})


def login_v(request):
    username = password = ''
    if request.POST:
        username = request.POST.get('username')
        password = request.POST.get('password')

        user = authenticate(username=username, password=password)
        if user is not None:
            if user.is_active:
                login(request, user)
                return HttpResponseRedirect('/home')

        else:
            return HttpResponseRedirect('/user/login.html')
    else:

        return HttpResponseRedirect('/user/login.html')

@login_required(login_url='/accounts/login/')
def create_post(request):
    template= 'user_app/create_post.html'
    context = {}
    return render(request, template, context)
@login_required(login_url='/accounts/login/')
def texts(request):
    texts = Text.objects.all()
    comments = Comment.objects.all()
    template = 'user_app/texts.html'
    context = {'texts': texts, 'comments': comments}
    return render(request, template, context)


@login_required(login_url='/accounts/login/')
def home(request):
    template = 'user_app/home.html'
    context = {}
    return render(request, template, context)


def index(request):

    if request.user.is_authenticated:
        return HttpResponseRedirect("/home/")
        #pass
    template = 'user_app/index.html'
    context = {}
    return render(request, template, context)


@login_required(login_url='/accounts/login/')
def logout_view(request):
    logout(request)
    return redirect('/')


class Register(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'registration/register.html'
    style = {'template_pack': 'rest_framework/vertical/'}

    def get(self, request):
        serializer = ProfileSerializerFlat()
        return Response({'serializer': serializer})

    def post(self, request):


        '''
        
        last_name
        gender
        date_of_birth
        '''
        serializer = ProfileSerializerFlat(data=request.data)
        if not serializer.is_valid():
            return Response({'serializer': serializer})

        serializer.save()
        return redirect('/home/')


def add_comment(request, post_id):
    if request.POST.get('click', False):
        pass
    else:
        pass

'''
def register(request):
    template = 'registration/register.html'
    if request.method == 'POST':

        if form.is_valid():
            form.save()
            username = form.cleaned_data['username']
            password = form.cleaned_data['password1']
            user = authenticate(username=username, password=password)
            login(request, user)
            return redirect('index')

    else:
        form = SignUpForm()

    context = {'form': form}
    return render(request, template, context)
'''