from django.conf.urls import url
from django.urls import path, include
from django.views.generic import TemplateView

from post_app.views import *
from post_app import views

urlpatterns = [
    path('', views.TextList.as_view()),
    path('create/', views.create_post, name="create_post"),

    path('texts/', views.texts, name='texts'),
    path('links/', views.links, name='links'),
    path('files/', views.files, name='files'),

    path('text/', TextView.as_view(), name='add_post_text'),
    path('file/', FileView.as_view(), name='add_post_file'),
    path('link/', LinkView.as_view(), name='add_post_link'),

    url('(texts|files|links)/(?P<post_id>[0-9]+)', views.post, name='post'),


    url("(?P<post_id>[0-9]+)/(?P<type>[a-zA-Z]+)/likes", views.like, name='add_like'),
    url("(?P<post_id>[0-9]+)/(?P<type>[a-zA-Z]+)/comments", CommentView.as_view(), name='add_comment'),




]