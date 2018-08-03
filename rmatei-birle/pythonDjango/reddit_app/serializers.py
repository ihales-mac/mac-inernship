from . models import *
from django import forms
from rest_framework import serializers
from django.contrib.auth.models import User


class UserDetailsSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = UserDetails
        fields = ('firstname', 'lastname', 'avatar', )


class CustomUserSerializer(serializers.HyperlinkedModelSerializer):
    user_details = UserDetailsSerializer(many=False)
    password = serializers.CharField(
        style={'input_type': 'password'},
        )

    class Meta:
        model = CustomUser
        fields = ('username', 'password', 'user_details', )


class SignupSerializer(serializers.HyperlinkedModelSerializer):
    firstname = serializers.CharField(
        source="reddit_app.UserDetails.firstname")
    lastname = serializers.CharField(source="reddit_app.UserDetails.lastname")
    avatar = serializers.ImageField(source="reddit_app.UserDetails.avatar")

    class Meta:
        model = CustomUser
        fields = ('username', 'password', 'firstname', 'lastname', 'avatar')

    def create(self, validated_data, photo):
        username = validated_data.pop('username')[0]
        password = validated_data.pop('password')[0]
        firstname = validated_data.pop('firstname')[0]
        lastname = validated_data.pop('lastname')[0]
        avatar = photo

        print("Username:{}, type:{}".format(username, type(username)))
        print("password:{}, type:{}".format(password, type(password)))
        print("firstname:{}, type:{}".format(firstname, type(firstname)))
        print("lastname:{}, type:{}".format(lastname, type(lastname)))
        print("avatar:{}, type:{}".format(avatar, type(avatar)))

        userdet = UserDetails.objects.create(
            firstname=firstname, lastname=lastname, avatar=avatar)
        userdet.save()

        usr = CustomUser.objects.create(
            username=username, user_details=userdet)
        usr.set_password(password)
        usr.save()

        return usr

    def update(self, validated_data, uid):
        user = CustomUser.objects.get(id=uid)

        username = validated_data.pop('username')[0]
        password = validated_data.pop('password')[0]
        firstname = validated_data.pop('user_details.firstname')[0]
        lastname = validated_data.pop('user_details.lastname')[0]
        try:
            avatar = request.FILES['avatar']
        except:
            avatar = None
        
        user.username = username
        user.user_details.firstname = firstname
        user.user_details.lastname = lastname

        if avatar:
            user.user_details.avatar = avatar
        if password != '':
            user.set_password(password)

        user.user_details.save()
        user.save()
        return user


class MakePostSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Post
        fields = ('text', 'photo')

    def create(self, validated_data, user, foto):
        user = user
        text = validated_data.pop('text')[0]
        photo = foto

        print("user:{}, type:{}".format(user, type(user)))
        print("text:{}, type:{}".format(text, type(text)))
        print("photo:{}, type:{}".format(photo, type(photo)))

        post = Post.objects.create(user=user, text=text, photo=photo)

        return post


class PostSerializer(serializers.HyperlinkedModelSerializer):
    def get_post(self, id):
        post = Post.objects.get(id=id)
        comments = Comment.objects.filter(post=post)
        return (post, comments)


class CommSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Comment
        fields = ('text',)

    def create(self, validated_data, user, post):
        user = user
        post = post
        text = validated_data.pop('text')[0]

        comm = Comment.objects.create(user=user, post=post, text=text)

        return comm


class LikeSerializer(serializers.HyperlinkedModelSerializer):

    def get_post_likes(self, pid):
        post = Post.objects.get(id=pid)
        try:
            likes = Like.objects.filter(post=post)
            return likes
        except:
            return None

    def get_user_likes(self, uid):
        user = CustomUser.objects.get(id=uid)
        try:
            likes = Like.objects.filter(user=user)            
            return likes
        except:
            return None

    def create(self, pid, uid):
        post = Post.objects.get(id=pid)
        user = CustomUser.objects.get(id=uid)
        try:
            like = Like.objects.get(post=post, user=user)
        except:
            like = Like.objects.create(post=post, user=user)
        return like