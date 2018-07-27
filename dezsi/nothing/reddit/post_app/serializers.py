from rest_framework import serializers

from post_app.models import *


class PosterSerializer(serializers.ModelSerializer):
    class Meta:
        model = Poster
        fields = '__all__'

class TextSerializer(serializers.ModelSerializer):
    class Meta:
        model = Text
        fields = '__all__'


class FileSerializer(serializers.ModelSerializer):
    class Meta:
        model = File
        fields = '__all__'


class LinkSerializer(serializers.ModelSerializer):
    class Meta:
        model = Link
        fields = '__all__'


class ProfileSerializerStruct(serializers.ModelSerializer):

    poster = PosterSerializer(many=True)

    class Meta:
        model = Profile
        fields = '__all__'

    def create(self, validated_data):
        poster_data = validated_data.pop('poster')
        poster = Poster.objects.create(poster_data)
        profile = Profile.objects.create(user = poster, first_name = validated_data.first_name, last_name = validated_data.last_name,
                               gender = validated_data.gender, date_of_birth = validated_data.date_of_birth)
        return profile


class ProfileSerializerFlat(serializers.ModelSerializer):
    def __init(self):

        self.username = serializers.CharField(source='Poster.username')
        self.password = serializers.CharField(source='Poster.password')
        self.email = serializers.CharField(source='Poster.email')
    class Meta:
        model = Profile
        fields = ('first_name', 'last_name','gender','date_of_birth')

    def create(self, validated_data):
        return Profile.objects.create(user = Poster.objects.create_user(self.username, self.email, self.password), first_name = validated_data.first_name, last_name = validated_data.last_name,
                               gender = validated_data.gender, date_of_birth = validated_data.date_of_birth)
