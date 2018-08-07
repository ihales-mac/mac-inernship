
from django.contrib.auth import authenticate, login
from django.contrib.auth.models import Permission

from django.db import transaction
from django.forms import PasswordInput
from django.shortcuts import redirect
from rest_framework import serializers
from rest_framework.authtoken.models import Token
from rest_framework.exceptions import ValidationError

from post_app.models import *
from reddit2_api import settings


class PostSerializer(serializers.ModelSerializer):
    class Meta:
        model = Post
        fields = [
            'id',
            'user',
            'title',
            'created_date',
            'title',
            'content',
            'image',
        ]

class PostCreateUpdateSerializer(serializers.ModelSerializer):
    class Meta:
        model = Post
        fields = [


            'title',
            'content',
            'image',
        ]



class PosterDetailsSerializer(serializers.ModelSerializer):


    class Meta:
        model = Poster
        fields = ('username','email')


class PosterSerializer(serializers.ModelSerializer):
    password = serializers.CharField(style={'input_type': 'password'})

    class Meta:
        model = Poster
        fields = ('username', 'password', 'email')




class CommentSerializer(serializers.ModelSerializer):
    class Meta:
        model = Comment
        fields = '__all__'


class ProfileSerializer(serializers.ModelSerializer):
    user = PosterDetailsSerializer()
    class Meta:
        model = Profile
        fields =('user','first_name', 'last_name','gender','date_of_birth','avatar')

class UserCreateSerializer(serializers.ModelSerializer):

    user = PosterSerializer()
    class Meta:
        model = Profile
        fields = ('user','first_name', 'last_name','gender','date_of_birth','avatar')


    def create(self, validated_data):
        user_data = validated_data.pop('user')
        with transaction.atomic():
            user = Poster.objects.create(
                username=user_data['username'],
                email=user_data['email'],

            )
            user.set_password(user_data['password'])
            user.save()

            return Profile.objects.create(user=user, **validated_data)


class CommentCreateSerializer(serializers.ModelSerializer):

    class Meta:
        model = Comment
        fields = ('comment',)


class LikeCreateSerializer(serializers.ModelSerializer):

    class Meta:
        model = Like
        fields = ()


class UserLoginSerializer(serializers.ModelSerializer):
    username = serializers.CharField()
    password = serializers.CharField()

    class Meta:
        model = Poster
        fields = ['username', 'password']
        extra_kwargs = {"password": {"write_only": True}}

    def validate(self, data):
        username = data['username']
        password = data['password']


        user = Poster.objects.get(username = username)
        if not user:
            raise ValidationError('This user name is not valid')

        if not user.check_password(password):
            raise ValidationError('This password is not valid')


        return user



class UserLogouterializer(serializers.ModelSerializer):

    class Meta:
        model = Poster
        fields = ['username', 'password']
        extra_kwargs = {"password": {"write_only": True}}

    def validate(self, data):
        username = data['username']
        password = data['password']


        user = Poster.objects.get(username = username)
        if not user:
            raise ValidationError('This user name is not valid')

        if not user.check_password(password):
            raise ValidationError('This password is not valid')


        return user





