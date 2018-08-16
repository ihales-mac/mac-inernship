from django.conf.urls import url
from django.contrib.auth.decorators import login_required
from django.contrib.auth.views import LoginView, LogoutView

from redditapp import views
from redditapp.views import PostListView, PostDetailsView, GiveCommentView, GiveLikeView, ProfileView

urlpatterns = [
    url(r'login$',
        LoginView.as_view(template_name='redditapp/login.html'),
        name='reddit_login'),
    url(r'logout$',
        LogoutView.as_view(), name='reddit_logout'),
    url(r'home/', login_required(PostListView.as_view()), name='reddit_home'),
    url(r'register$', views.SignUp.as_view(), name='reddit_sign_up'),
    url(r'create-post$', login_required(views.CreatePostView.as_view()), name='reddit_create_post'),
    url(r'^post-detail/(?P<pk>[0-9]+)/$', login_required(PostDetailsView.as_view()), name='reddit_post_details'),
    url(r'^give-comment/(?P<pk>[0-9]+)/$', login_required(GiveCommentView.as_view()), name='reddit_give_comment'),
    url(r'^give-like/(?P<pk>[0-9]+)/$', login_required(GiveLikeView.as_view()), name='reddit_give_like'),
    url(r'^user-profile/$', ProfileView.as_view(), name='reddit_user_profile')
]
