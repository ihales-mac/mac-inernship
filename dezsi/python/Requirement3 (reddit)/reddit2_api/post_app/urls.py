from django.conf.urls import url
from django.urls import path, include
from django.views.generic import TemplateView

from post_app.views import *
from post_app import views

urlpatterns = [
    path('', PostListViewAPI.as_view(), name='list'),

    path('create/', PostCreateAPIView.as_view(), name='create'),
    url(r'^(?P<pk>\d+)/$', PostDetailAPIView.as_view(), name='detail'),
    url(r'^(?P<pk>\d+)/delete$', PostDeleteAPIView.as_view(), name='delete'),
    url(r'^(?P<pk>\d+)/edit$', PostUpdateAPIView.as_view(), name='update'),
    url(r'^(?P<post>\w+)/comment$', CommentCreateAPIView.as_view(), name='create_comment'),
    url(r'^(?P<post>\w+)/like/', LikeCreateAPIView.as_view(), name='create_like'),


]