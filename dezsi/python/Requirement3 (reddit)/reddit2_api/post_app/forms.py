from django import forms
from pagedown.widgets import PagedownWidget

from post_app.models import Post


class PostForm(forms.ModelForm):
    content = forms.CharField(widget=PagedownWidget(show_preview = False))
    publish = forms.CharField(widget=forms.SelectDateWidget)
    class Meta:
        model = Post
        fields = [
            'title',
            'content',
            'image',
            'publish',

        ]




class CommentForm(forms.Form):

    comment = forms.CharField( widget=forms.Textarea(attrs={'placeholder': 'Type your comment here','rows': 4, 'cols': 50}) )
