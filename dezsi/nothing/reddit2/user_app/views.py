from pprint import pprint

from django.contrib.auth import authenticate, login, logout
from django.contrib.auth.decorators import login_required
from django.http import HttpResponseRedirect
from django.shortcuts import render, redirect, get_object_or_404

# Create your views here.
from django.shortcuts import render

from rest_framework.renderers import TemplateHTMLRenderer
from rest_framework.response import Response
from rest_framework.views import APIView

from post_app.models import *
from post_app.serializers import *
from django.contrib import admin
from user_app.forms_old import SignUpForm
from post_app.models import Profile
from user_app.serializers import ProfileSerializerFlat, ProfileSerializerStruct


class DetailsList(APIView):

    renderer_classes = [TemplateHTMLRenderer]
    template_name = "user_app/details.html"

    def get(self, request):
        entities = Poster.objects.all().filter(username__iexact = request.user.username)

        serializer = PosterSerializer(entities, many=True)
        pprint({'details': serializer.data})
        return Response({'details': serializer.data})


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


@login_required(login_url='/accounts/login/')
def home_x(request):
    template = 'user_app/home.html'
    context = {}
    return render(request, template, context)


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