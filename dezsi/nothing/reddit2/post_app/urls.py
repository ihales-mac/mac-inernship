from django.urls import path, include

from post_app import views

urlpatterns = [
    path('', views.TextList.as_view()),
]