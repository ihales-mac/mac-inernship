from django import forms


class BaseForm(forms.Form):
    title = forms.CharField(label="Title", max_length=250)
    original_content = forms.BooleanField(required=False, initial=False)
    spoiler = forms.BooleanField(required=False, initial=False)
    nsfw = forms.BooleanField(required=False, initial=False)


class TextForm(BaseForm):
    text = forms.CharField(empty_value='', widget=forms.Textarea, max_length=1000)


class FileForm(BaseForm):
    file = forms.FileField()


class LinkForm(BaseForm):
    link = forms.URLField()


class CommentForm(forms.Form):

    comment = forms.CharField( widget=forms.Textarea(attrs={'placeholder': 'Type your comment here','rows': 4, 'cols': 50}) )
