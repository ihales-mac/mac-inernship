from django.conf.urls import url
from django.urls import path, include

from redditapp.views import PostListView, PostDetailView, UserCreateView, UserLoginView, UserProfileView, \
    CreatePostView, GiveCommentView, GiveLikeView

urlpatterns = [
    url(r'^posts/$', PostListView.as_view(), name='posts_list'),
    url(r'^post-detail/(?P<pk>[0-9]+)/$', PostDetailView.as_view(), name='post_detail'),
    url(r'^user-profile/(?P<username>[a-zA-Z0-9]+)/$', UserProfileView.as_view(), name='profile_view'),

    # path('auth/',include('rest_framework.urls'))
    # path('auth/login/', LoginView.as_view(), name='reddit_login'),
    # path('auth/logout/', LogoutView.as_view(), name='reddit_logout'),

    url(r'^register/$', UserCreateView.as_view(), name='reddit_register'),
    url(r'^login/$', UserLoginView.as_view(), name='reddit_login'),
    url(r'^create-post/$', CreatePostView.as_view(), name='reddit_create_post'),
    url(r'^give-comment/(?P<pk>[0-9]+)/$', GiveCommentView.as_view(), name='reddit_give_comment'),
    url(r'^give-like/(?P<pk>[0-9]+)/$', GiveLikeView.as_view(), name='reddit_give_like')
]
