from django.db import models
from django.contrib.auth.models import AbstractUser
from django.utils import timezone


class UserDetails(models.Model):
    firstname = models.CharField(max_length=40)
    lastname = models.CharField(max_length=40)
    avatar = models.ImageField(blank=True, upload_to="avatars")
    created = models.DateTimeField(default=None)
    modified = models.DateTimeField(default=None)

    def save(self, *args, **kwargs):
        ''' On save, update timestamps '''
        if not self.created:
            self.created = timezone.now()
        self.modified = timezone.now()
        return super(UserDetails, self).save(*args, **kwargs)

    def __str__(self):
        return (self.firstname + " " + self.lastname)

    class Meta:
        ordering = ('firstname', 'lastname',)


class CustomUser(AbstractUser):
    user_details = models.OneToOneField(UserDetails, on_delete=models.CASCADE)
    created = models.DateTimeField(default=None)
    modified = models.DateTimeField(default=None)

    def save(self, *args, **kwargs):
        ''' On save, update timestamps '''
        if not self.created:
            self.created = timezone.now()
        self.modified = timezone.now()
        return super(CustomUser, self).save(*args, **kwargs)


class Post(models.Model):
    user = models.ForeignKey(CustomUser, on_delete=models.CASCADE)
    text = models.CharField(max_length=2000)
    photo = models.ImageField(blank=True, upload_to="gallery")
    created = models.DateTimeField(default=None)
    modified = models.DateTimeField(default=None)

    def save(self, *args, **kwargs):
        ''' On save, update timestamps '''
        if not self.created:
            self.created = timezone.now()
        self.modified = timezone.now()
        return super(Post, self).save(*args, **kwargs)

    def __str__(self):
        return self.text
        
    class Meta:
        ordering = ('created', )


class Comment(models.Model):
    user = models.ForeignKey(CustomUser, on_delete=models.CASCADE)
    post = models.ForeignKey(Post, on_delete=models.CASCADE)
    text = models.CharField(max_length=400)
    created = models.DateTimeField(default=None)
    modified = models.DateTimeField(default=None)

    def save(self, *args, **kwargs):
        ''' On save, update timestamps '''
        if not self.created:
            self.created = timezone.now()
        self.modified = timezone.now()
        return super(Comment, self).save(*args, **kwargs)

    def __str__(self):
        return self.text

    class Meta:
        ordering = ('created', )


class Like(models.Model):
    user = models.ForeignKey(CustomUser, on_delete=models.CASCADE)
    post = models.ForeignKey(Post, on_delete=models.CASCADE)
    created = models.DateTimeField(default=None)
    modified = models.DateTimeField(default=None)

    def save(self, *args, **kwargs):
        ''' On save, update timestamps '''
        if not self.created:
            self.created = timezone.now()
        self.modified = timezone.now()
        return super(Like, self).save(*args, **kwargs)

    class Meta:
        ordering = ('created', )