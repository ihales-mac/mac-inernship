from django import forms
from django.contrib.auth.models import User, AbstractUser
from django.contrib.contenttypes.fields import GenericForeignKey, GenericRelation
from django.contrib.contenttypes.models import ContentType
from django.db import models

# Create your models here.
from django.db.models import CASCADE
from django.utils.datetime_safe import datetime



class Poster(AbstractUser):
    pass



class Profile(models.Model):
    objects = models.Manager()
    user = models.OneToOneField(Poster, on_delete=models.CASCADE, related_name='profile')
    first_name = models.CharField(max_length=50, blank = True)
    last_name = models.CharField(max_length=50, blank=True)
    gender = models.CharField(max_length=6, blank=True, default='')
    date_of_birth = models.DateField(blank=True, default='1900-01-01')
    avatar = models.FileField(default='default_pic.jpg')


class Post(models.Model):
    objects = models.Manager()
    user = models.ForeignKey(Poster, on_delete=CASCADE)
    created_date = models.DateTimeField(default=datetime.now)
    title = models.CharField(max_length=250)
    content = models.CharField(max_length= 1000, blank=True)
    image = models.FileField(blank=True)
    nr_likes = models.IntegerField(default=0)
    def __str__(self):
        return self.title


class Comment(models.Model):
    objects = models.Manager()
    user = models.ForeignKey(Profile, on_delete=models.CASCADE)
    created_date = models.DateTimeField(default=datetime.now)
    comment = models.CharField(max_length=250, default='', blank=True)
    post = models.ForeignKey(Post, on_delete=CASCADE)


class Like(models.Model):
    objects = models.Manager()
    user = models.ForeignKey(Poster, on_delete=models.CASCADE)
    created_date = models.DateTimeField(default=datetime.now)
    post = models.ForeignKey(Post, on_delete=CASCADE)
