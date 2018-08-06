"""reddit2_url URL Configuration

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
from django.conf.urls import url
from django.conf.urls.static import static
from django.contrib import admin
from django.urls import path, include

from post_app.views import UserDetailsList
from reddit2_api import settings
from reddit2_api.views import UserCreateAPIView

urlpatterns = [

    path('admin/', admin.site.urls),
    path('accounts/', include('django.contrib.auth.urls')),
    path('posts/', include('post_app.urls')),
    path('register/', UserCreateAPIView.as_view(), name='register'),
    url(r'^users/(?P<pk>\d+)$', UserDetailsList.as_view(), name='user_details'),
] +static(settings.MEDIA_URL, document_root = settings.MEDIA_ROOT)

