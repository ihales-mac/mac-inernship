"""FakeRedditApp URL Configuration

The `urlpatterns` list routes URLs to views. For more information please see:
    https://docs.djangoproject.com/en/2.0/topics/http/urls/
Examples:
Function views
    1. Add an import:  from my_app import views
    2. Add a URL to urlpatterns:  path('', views.home, name='home')
Class-based views
    1. Add an import:  from other_app.views import Home
    2. Add a URL to urlpatterns:  path('', Home.as_view(), name='home')
Including another URLconf
    1. Import the include() function: from django.urls import include, path
    2. Add a URL to urlpatterns:  path('blog/', include('blog.urls'))
"""
from django.contrib import admin
from django.urls import path
from django.conf.urls import url, include
from rest_framework import routers
from FakeReddit import views
from django.views.static import serve
from django.contrib.auth.views import LoginView, LogoutView
from django.conf.urls.static import static
from django.contrib.staticfiles.urls import staticfiles_urlpatterns
from . import settings

urlpatterns = [
    url(r'viewpost/(?P<pid>\d+)/$', views.viewpost.as_view(), name="viewpost"),
    url(r'^makepost/$', views.makepost.as_view(), name='makepost'),
    url(r'^login/$', LoginView.as_view(template_name='login.html'), name='login'),
    url(r'^logout/$', LogoutView.as_view(), name='logout'),
    url(r'^editprofile/$', views.editprofile.as_view(), name="editprofile"),
    url(r'^signup/$', views.signup.as_view(), name='signup'),
    url(r'^$', views.index.as_view(), name='index'),
]

if settings.DEBUG:
    root = settings.MEDIA_ROOT
    urlpatterns.append(
        url(r'^media/(?P<path>.*)$', serve, {'document_root': root}))


urlpatterns += static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)
urlpatterns += staticfiles_urlpatterns()
