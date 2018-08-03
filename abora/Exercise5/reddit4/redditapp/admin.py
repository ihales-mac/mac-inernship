from django.contrib import admin

from redditapp.models import *

admin.site.register(Post)
admin.site.register(Comment)
admin.site.register(Like)
admin.site.register(CustomUser)
admin.site.register(UserDetails)
