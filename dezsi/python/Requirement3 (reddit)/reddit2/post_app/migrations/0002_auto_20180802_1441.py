# Generated by Django 2.0.7 on 2018-08-02 11:41

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('post_app', '0001_initial'),
    ]

    operations = [
        migrations.AlterField(
            model_name='profile',
            name='avatar',
            field=models.FileField(default='default_pic.jpg', upload_to=''),
        ),
    ]
