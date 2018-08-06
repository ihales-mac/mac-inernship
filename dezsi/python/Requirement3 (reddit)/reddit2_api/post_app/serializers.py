from django.db import transaction
from django.forms import PasswordInput
from rest_framework import serializers

from post_app.models import *


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
        fields = ('comment','post')

