# Generated by Django 2.0.7 on 2018-08-08 08:19

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('post_app', '0001_initial'),
    ]

    operations = [
        migrations.AddField(
            model_name='post',
            name='nr_likes',
            field=models.IntegerField(default=0),
        ),
    ]
