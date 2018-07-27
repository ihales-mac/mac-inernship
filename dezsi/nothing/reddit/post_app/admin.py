from django.contrib import admin

# Register your models here.
from post_app import models

admin.site.register(models.Text)
admin.site.register(models.File)
admin.site.register(models.Link)
admin.site.register(models.Poster)
admin.site.register(models.Profile)
admin.site.register(models.Comment)
admin.site.register(models.Like)


