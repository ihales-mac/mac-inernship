from pprint import pprint

from django.shortcuts import render

# Create your views here.
from django.views.generic import DetailView
from rest_framework.renderers import JSONRenderer, TemplateHTMLRenderer
from rest_framework.response import Response
from rest_framework.views import APIView

from post_app.serializers import *
from . models import Text


class TextList(APIView):

    renderer_classes = [TemplateHTMLRenderer]
    template_name = "C:\\Users\\Intern\\PycharmProjects\\reddit2\\user_app\\templates\\user_app\\home.html"
    def get(self, request):
        texts = Text.objects.all()
        serializer = TextSerializer(texts, many=True)
        pprint({'texts': serializer.data})
        return Response({'texts': serializer.data})


