from . models import *
from rest_framework import serializers
from django.contrib.auth.models import User


class UserDetailsSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = UserDetails
        fields = ('firstname', 'lastname', 'avatar', )


class CustomUserSerializer(serializers.HyperlinkedModelSerializer):
    user_details = UserDetailsSerializer(many=True)

    class Meta:
        model = CustomUser
        fields = ('username', 'password', )


class ProfileSerializer(serializers.HyperlinkedModelSerializer):
    firstname = serializers.CharField(source="user_details.firstname")
    lastname = serializers.CharField(source="user_details.lastname")
    avatar = serializers.ImageField(source="user_details.avatar")

    class Meta:
        model = CustomUser
        fields = ('username', 'password', 'firstname', 'lastname', 'avatar')

    def create(self, validated_data, photo):
        un = validated_data.pop('username')[0]
        pw = validated_data.pop('password')[0]
        fn = validated_data.pop('firstname')[0]
        ln = validated_data.pop('lastname')[0]
        av = photo
        ud = UserDetails.objects.create(firstname=fn, lastname=ln, avatar=av)
        ud.save()
        usr = CustomUser.objects.create(username=un, user_details=ud)
        usr.set_password(pw)
        usr.save()
        return usr


class EditProfileSerializer(serializers.HyperlinkedModelSerializer):
    firstname = serializers.CharField(source="user_details.firstname")
    lastname = serializers.CharField(source="user_details.lastname")
    avatar = serializers.ImageField(source="user_details.avatar")

    class Meta:
        model = CustomUser
        fields = ('username', 'firstname', 'lastname', 'avatar')

    def edit(self, validated_data, id, photo):
        thisuser = CustomUser.objects.get(id=id)
        thisuser.username = validated_data.pop('username')[0]
        thisuser.save()
        thisUserDetails = thisuser.user_details
        thisUserDetails.firstname = validated_data.pop('firstname')[0]
        thisUserDetails.lastname = validated_data.pop('lastname')[0]
        thisUserDetails.avatar = photo
        thisUserDetails.save()
        return thisuser


class MakePostSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Post
        fields = ('text', 'photo')

    def create(self, validated_data, user, photo):
        text = validated_data.pop('text')[0]
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
        text = validated_data.pop('text')[0]
        comm = Comment.objects.create(user=user, post=post, text=text)
        return comm


class LikeSerializer(serializers.HyperlinkedModelSerializer):

    def create(self, user, post):
        like = Like.objects.create(user=user, post=post)
        return like
