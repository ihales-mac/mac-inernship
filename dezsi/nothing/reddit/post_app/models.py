from django.contrib.auth.models import AbstractUser
from django.db import models

# Create your models here.


class Poster(AbstractUser):
    pass


class Like(models.Model):
    user = models.ForeignKey(Poster, on_delete=models.CASCADE)
    created_date = models.DateTimeField()


class Comment(models.Model):
    user = models.ForeignKey(Poster, on_delete=models.CASCADE)
    created_date = models.DateTimeField()


class Post(models.Model):
    created_date = models.DateTimeField()

    title = models.CharField(max_length=250)
    original_content = models.BooleanField(blank=True, default=False)
    spoiler = models.BooleanField(blank=True, default=False)
    nsfw = models.BooleanField(blank=True, default=False)
    likes = models.IntegerField(default=0)

    class Meta:
        abstract = True

    def __str__(self):
        return self.title


class Text(Post):
    user = models.ForeignKey(Poster, on_delete=models.CASCADE)
    text = models.CharField(max_length=1000, default='', blank=True)


class File(Post):
    user = models.ForeignKey(Poster, on_delete=models.CASCADE)
    file = models.FileField()


class Link(Post):
    user = models.ForeignKey(Poster, on_delete=models.CASCADE)
    url = models.URLField()


class Profile(models.Model):
    user = models.OneToOneField(Poster, on_delete="models.CASCADE")
    first_name = models.CharField(max_length=50, blank = True)
    last_name = models.CharField(max_length=50, blank=True)
    gender = models.CharField(max_length=6, blank=True, default='')
    date_of_birth = models.DateField(blank=True, default='1990-01-01')


