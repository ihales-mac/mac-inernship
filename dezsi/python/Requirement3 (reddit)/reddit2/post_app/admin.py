from django.contrib import admin

# Register your models here.
from post_app.models import *

admin.site.register(Text)
admin.site.register(File)
admin.site.register(Link)
admin.site.register(Poster)
admin.site.register(Profile)
admin.site.register(Comment)
admin.site.register(Like)


