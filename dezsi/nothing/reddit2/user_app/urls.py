from django.urls import path, include
from django.views.generic import TemplateView

from user_app import views
urlpatterns = [

    path('', views.index, name='index'),
    path('home/', views.home, name='home'),
    path('register/', views.Register.as_view(), name='register'),
    path('logout/', views.logout_view, name='logout_view'),
    path('details/', views.DetailsList.as_view(), name='personal_data'),
    path('create/', views.create_post, name="create_post"),
    path('login/', views.login_v, name='login'),
    path('login.html',  TemplateView.as_view(template_name='registration/login.html'), name="static_login"),
    path('comment/', views.add_comment ,name='comment' ),
    path('texts/', views.texts, name='texts'),
    path('links/', TemplateView.as_view(template_name='user_app/links.html'), name='links'),
    path('files/', TemplateView.as_view(template_name='user_app/files.html'), name='files'),

]
