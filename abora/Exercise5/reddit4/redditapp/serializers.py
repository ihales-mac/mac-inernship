from rest_framework import serializers
from rest_framework.serializers import SerializerMethodField

from redditapp.models import CustomUser, UserDetails, Post, Like, Comment
from django.db import transaction


class UserDetailsSerializer(serializers.ModelSerializer):
    class Meta:
        model = UserDetails
        fields = ('first_name', 'last_name', 'email')


class CustomUserSerializerPost(serializers.ModelSerializer):
    first_name = serializers.CharField(source='UserDetails.first_name')
    last_name = serializers.CharField(source='UserDetails.last_name')
    email = serializers.CharField(source='UserDetails.email')
    avatar = serializers.ImageField(source='UserDetails.avatar')
    password = serializers.CharField(
        style={'input_type': 'password', 'placeholder': 'Password'}
    )

    class Meta:
        model = CustomUser
        fields = ('username', 'password', 'first_name', 'last_name', 'email', 'avatar')

    def create(self, validated_data):
        profile_data = validated_data.pop('UserDetails')
        with transaction.atomic():
            custom_user = CustomUser(username=validated_data['username'])
            custom_user.set_password(validated_data['password'])
            custom_user.save()
            # custom_user = CustomUser.objects.create(username=validated_data['username'],password=validated_data['password'])
            return UserDetails.objects.create(user=custom_user, first_name=profile_data['first_name'],
                                              last_name=profile_data['last_name'], email=profile_data['email'],
                                              avatar=profile_data['avatar'])


class LikeSerializer(serializers.ModelSerializer):
    class Meta:
        model = Like
        fields = ('created_date', 'owner')


class UserSerializer(serializers.ModelSerializer):
    avatar = SerializerMethodField()

    class Meta:
        model = CustomUser
        fields = ('id', 'username', 'avatar')

    def get_avatar(self, obj):
        if obj.userdetails.avatar:
            return obj.userdetails.avatar.url
        return None


class CommentSerializer(serializers.ModelSerializer):
    owner = UserSerializer()

    class Meta:
        model = Comment
        fields = ('text', 'owner')


class PostSerializer(serializers.ModelSerializer):
    comments = CommentSerializer(many=True)
    likes = LikeSerializer(many=True)
    owner = UserSerializer()

    class Meta:
        model = Post
        fields = ('id', 'category', 'posted_date', 'title', 'image', 'comments', 'likes', 'owner')


class CreatePostSerializer(serializers.ModelSerializer):
    class Meta:
        model = Post
        fields = ('category', 'posted_date', 'title', 'image')


class GiveCommentSerializer(serializers.ModelSerializer):
    class Meta:
        model = Comment
        fields = ('text',)


class GiveLikeSerializer(serializers.ModelSerializer):
    class Meta:
        model = Like
        fields = ('created_date',)


class ProfileSerializer(serializers.ModelSerializer):
    # user = UserSerializer()
    user = SerializerMethodField()

    class Meta:
        model = UserDetails
        fields = ('first_name', 'last_name', 'email', 'user', 'avatar')

    def get_user(self, obj):
        return str(obj.user.username)
