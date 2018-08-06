from django.urls import path
from django.views.generic import TemplateView

import post_app.views
import user_app.views
from post_app.views import TextView
from user_app import views

urlpatterns = [

    path('', views.index, name='index'),
    path('home/', views.home, name='home'),
    path('register/', views.Register.as_view(), name='register'),
    path('logout/', views.logout_view, name='logout_view'),
    path('details/', user_app.views.DetailsList.as_view(), name='personal_data'),
    path('login/', views.login_v, name='login'),
    path('login.html',  TemplateView.as_view(template_name='registration/login.html'), name="static_login"),


]
