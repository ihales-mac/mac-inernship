from django.contrib.auth.models import User, AbstractUser
from django.contrib.contenttypes.fields import GenericForeignKey, GenericRelation
from django.contrib.contenttypes.models import ContentType
from django.db import models

# Create your models here.
from django.db.models import CASCADE


class Poster(AbstractUser):
    pass





class Post(models.Model):
    created_date = models.DateTimeField()

    title = models.CharField(max_length=250)
    original_content = models.BooleanField(blank=True, default=False)
    spoiler = models.BooleanField(blank=True, default=False)
    nsfw = models.BooleanField(blank=True, default=False)
    nr_likes = models.IntegerField(default=0)

    class Meta:
        abstract = True

    def __str__(self):
        return self.title


class Comment(models.Model):
    user = models.ForeignKey(Poster, on_delete=models.CASCADE)
    created_date = models.DateTimeField()
    comment = models.CharField(max_length=250, default='', blank=True)

 # Following fields are required for using GenericForeignKey
    content_type = models.ForeignKey(ContentType, on_delete=CASCADE, blank=True)
    object_id = models.PositiveIntegerField()
    post = GenericForeignKey()

class Like(models.Model):
    user = models.ForeignKey(Poster, on_delete=models.CASCADE)
    created_date = models.DateTimeField()

    # Following fields are required for using GenericForeignKey
    content_type = models.ForeignKey(ContentType, on_delete=CASCADE)
    object_id = models.PositiveIntegerField()
    post = GenericForeignKey()

class Text(Post):
    user = models.ForeignKey(Poster, on_delete=models.CASCADE)
    text = models.CharField(max_length=1000, default='', blank=True)
    likes = GenericRelation(Like)
    comments = GenericRelation(Comment)


class File(Post):
    user = models.ForeignKey(Poster, on_delete=models.CASCADE)
    file = models.FileField()
    likes = GenericRelation(Like)
    comments = GenericRelation(Comment)

class Link(Post):
    user = models.ForeignKey(Poster, on_delete=models.CASCADE)
    url = models.URLField()
    likes = GenericRelation(Like)
    comments = GenericRelation(Comment)



class Profile(models.Model):
    objects = models.Manager()
    user = models.OneToOneField(Poster, on_delete=models.CASCADE, related_name='profile')
    first_name = models.CharField(max_length=50, blank = True)
    last_name = models.CharField(max_length=50, blank=True)
    gender = models.CharField(max_length=6, blank=True, default='')
    date_of_birth = models.DateField(blank=True, default='1900-01-01')





