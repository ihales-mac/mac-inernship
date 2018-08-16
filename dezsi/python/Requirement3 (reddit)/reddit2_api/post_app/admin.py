from django.contrib import admin

# Register your models here.
from post_app.models import *

admin.site.register(Poster)
admin.site.register(Profile)
admin.site.register(Comment)
admin.site.register(Like)
admin.site.register(Post)


