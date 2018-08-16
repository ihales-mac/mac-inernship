from django.contrib.auth import authenticate
from rest_framework import serializers, exceptions
from rest_framework.authtoken.models import Token
from rest_framework.fields import SerializerMethodField
from redditapp.models import Post, Like, Comment, CustomUser, UserDetails
from django.db.models import Q
from django.db import transaction
import json
from rest_framework.validators import UniqueTogetherValidator

class PostListSerializer(serializers.ModelSerializer):
    owner = SerializerMethodField()
    like_count = SerializerMethodField()

    class Meta:
        model = Post
        fields = ('id', 'category', 'posted_date', 'title', 'image', 'owner', 'like_count')

    def get_owner(self, obj):
        return str(obj.owner.username)

    def get_like_count(self, obj):
        return obj.likes.count()


class LikeSerializer(serializers.ModelSerializer):
    owner = SerializerMethodField()

    class Meta:
        model = Like
        fields = ('create_date', 'owner')

    def get_owner(self, obj):
        return str(obj.owner.username)


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
        fields = ('id', 'text', 'posted_date', 'owner')


class PostDetailSerializer(serializers.ModelSerializer):
    likes = LikeSerializer(many=True)
    comments = CommentSerializer(many=True)
    owner = SerializerMethodField()

    class Meta:
        model = Post
        fields = ('id', 'category', 'posted_date', 'title', 'image', 'owner', 'likes', 'comments')

    def get_owner(self, obj):
        return str(obj.owner.username)


class UserPostsSerializer(serializers.ModelSerializer):
    class Meta:
        model = Post
        fields = ('id', 'category', 'posted_date', 'title', 'image')


class UserDetailsSerializer(serializers.ModelSerializer):
    class Meta:
        model = UserDetails
        fields = (
            'first_name',
            'last_name',
            'email',
            'avatar',
            'join_date'
        )


class UserProfileSerializer(serializers.ModelSerializer):
    posts = UserPostsSerializer(many=True)
    # Model name in lower case to work
    userdetails = UserDetailsSerializer()
    lookup_field = 'username'

    class Meta:
        model = CustomUser
        fields = ('id', 'username', 'posts', 'userdetails')


class UserLoginSerializer(serializers.ModelSerializer):
    token = serializers.CharField(allow_blank=True, read_only=True)
    # I get error if I don't override username
    username = serializers.CharField(required=False, allow_blank=True)

    # email = serializers.EmailField(label='Email Address', required=False, allow_blank=True)

    class Meta:
        model = CustomUser
        fields = [
            'username',
            'password',
            'token'
        ]

    def validate(self, data):
        # user_obj = None
        # email = data.get('email', None)
        # username = data.get('username', None)
        # password = data['password']
        # if not email or not username:
        #     raise exceptions.ValidationError('A user or email is required')
        #
        # user = CustomUser.objects.filter(
        #     Q(email=email) |
        #     Q(username=username)
        # ).distinct()
        #
        # if user.exits() and user.count() == 1:
        #     user_obj = user.first()
        # else:
        #     raise exceptions.ValidationError("This username/email is not valid.")
        #
        # if user_obj:
        #     if not user_obj.check_password(password):
        #         raise serializers.ValidationError("Incorrect credentials please try again.")
        # data['token'] = 'Some random token'
        # return data

        user_obj = None
        username = data.get('username', None)
        password = data['password']
        if not username:
            raise exceptions.ValidationError('A user is required')
        user_obj = CustomUser.objects.filter(username=username)
        if not user_obj.exists():
            raise serializers.ValidationError("The username is not valid")
        else:
            user_obj = user_obj.first()
            print(user_obj.pk)
        if not user_obj.check_password(password):
            raise serializers.ValidationError("Incorrect credentials please try again.")

        token, created = Token.objects.get_or_create(user=user_obj)
        data['token'] = token.key
        return data


class UserCreateSerializer(serializers.ModelSerializer):
    first_name = serializers.CharField(source='UserDetails.first_name')
    last_name = serializers.CharField(source='UserDetails.last_name')
    email = serializers.CharField(source='UserDetails.email')
    avatar = serializers.ImageField(source='UserDetails.avatar')

    class Meta:
        model = CustomUser
        fields = (
            'username',
            'password',
            'first_name',
            'last_name',
            'email',
            'avatar'
        )

    def create(self, validated_data):
        with transaction.atomic():
            profile_data = validated_data.pop("UserDetails")
            user_obj = CustomUser(username=validated_data['username'])
            user_obj.set_password(validated_data['password'])
            user_obj.save()
            user_details_obj = UserDetails(user=user_obj, first_name=profile_data['first_name'],
                                           last_name=profile_data['last_name'], email=profile_data['email'],
                                           avatar=profile_data['avatar'])
            user_details_obj.save()
            return user_details_obj


class CreatePostSerializer(serializers.ModelSerializer):
    class Meta:
        model = Post
        fields = ('category', 'title', 'image',)


class GiveCommentSerializer(serializers.ModelSerializer):
    class Meta:
        model = Comment
        fields = ('text',)


class GiveLikeSerializer(serializers.ModelSerializer):
    class Meta:
        model = Like
        fields = ('create_date',)

# Version 2
# class LoginSerializer(serializers.Serializer):
#     username = serializers.CharField()
#     password = serializers.CharField()
#
#     def validate(self, data):
#         username = data.get('username', '')
#         password = data.get('password', '')
#
#         print(username)
#         if username and password:
#             user = authenticate(username=username, password=password)
#             print(user)
#             if user:
#                 data['user'] = user
#             else:
#                 raise exceptions.ValidationError('Incorrect username or password')
#         else:
#             msg = 'Must provide username and password'
#             raise exceptions.ValidationError(msg)
#
#         return data
