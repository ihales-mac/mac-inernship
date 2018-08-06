from django.contrib.auth.models import User
from django.db import models


class CustomUser(User):
    pass
    # def save(self, *args, **kwargs):
    #     super().save(*args, **kwargs)


class UserDetails(models.Model):
    user = models.OneToOneField(CustomUser, on_delete=models.CASCADE, primary_key=True)
    first_name = models.CharField(max_length=50)
    last_name = models.CharField(max_length=50)
    email = models.CharField(max_length=50)
    avatar = models.ImageField(upload_to='avatar_image',blank=True,null=True)

    def __str__(self):
        return self.first_name


class Post(models.Model):
    category = models.CharField(max_length=50)
    posted_date = models.DateTimeField(auto_now_add=True)
    title = models.CharField(max_length=50)
    image = models.ImageField(upload_to='post_image', blank=True, null=True)
    owner = models.ForeignKey(CustomUser,on_delete=models.CASCADE)

    def __str__(self):
        return self.title


class Like(models.Model):
    created_date = models.DateTimeField(auto_now_add=True)
    owner = models.ForeignKey(CustomUser, on_delete=models.CASCADE)
    post = models.ForeignKey(Post,related_name='likes', on_delete=models.CASCADE)



class Comment(models.Model):
    text = models.TextField()
    owner = models.ForeignKey(CustomUser, on_delete=models.CASCADE)
    post = models.ForeignKey(Post,related_name='comments',on_delete=models.CASCADE)

    def __str__(self):
        return self.text
