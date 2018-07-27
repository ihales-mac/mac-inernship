from django.contrib import admin
from django.contrib.auth.admin import UserAdmin
from django.db import transaction
from rest_framework import serializers

from post_app.models import Poster
from post_app.serializers import PosterSerializer
from post_app.models import Profile


class ProfileSerializerStruct(serializers.ModelSerializer):

    poster= PosterSerializer(many=True).data

    class Meta:
        model = Profile
        fields = ('first_name', 'last_name', 'gender', 'date_of_birth')

    def create(self, validated_data):
        poster_data = validated_data.pop('poster')
        poster = Poster.objects.create(poster_data)
        profile = Profile.objects.create(user = poster, first_name = validated_data.first_name, last_name = validated_data.last_name,
                               gender = validated_data.gender, date_of_birth = validated_data.date_of_birth)
        return profile


class ProfileSerializerFlat(serializers.ModelSerializer):

    username = serializers.CharField(source='Poster.username')
    password = serializers.CharField(source='Poster.password')
    email = serializers.CharField(source='Poster.email')
    class Meta:
        model = Profile
        fields = ('username','password','email','first_name', 'last_name','gender','date_of_birth')




    def create(self, validated_data):
        poster = validated_data.pop('Poster')

        with transaction.atomic():
            poster = PosterSerializer.create(PosterSerializer(), validated_data=poster)

            return Profile.objects.create(user=poster, **validated_data)