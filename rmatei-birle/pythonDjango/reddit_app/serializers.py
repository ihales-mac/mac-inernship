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
