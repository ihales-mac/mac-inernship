from django.urls import path, include
from user_app import views
urlpatterns = [

    path('', views.index, name='index'),
    path('home/', views.home, name='home'),
    path('register/', views.Register.as_view(), name='register'),
    path('logout/', views.logout_view, name='logout_view'),
    path('details/', views.DetailsList.as_view(), name='personal_data')
]
