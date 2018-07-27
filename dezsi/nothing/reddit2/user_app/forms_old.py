from django import forms
from django.contrib.auth.forms import UserCreationForm
from django.contrib.auth.models import User



class SignUpForm(UserCreationForm):
    def __init__(self,obj=None):
        super(UserCreationForm, self).__init__(obj)
    DOY = ('1980', '1981', '1982', '1983', '1984', '1985', '1986', '1987',
           '1988', '1989', '1990', '1991', '1992', '1993', '1994', '1995',
           '1996', '1997', '1998', '1999', '2000', '2001', '2002', '2003',
           '2004', '2005', '2006', '2007', '2008', '2009', '2010', '2011',
           '2012', '2013', '2014', '2015')
    class Meta:
        model = None
        fields = ['first_name', 'last_name', 'email', 'date_of_birth', 'gender', 'username', 'password']

    first_name = forms.CharField(max_length=30, initial = '',required=False, help_text='Optional.')
    last_name = forms.CharField(max_length=30, initial='', required=False, help_text='Optional.')
    email = forms.EmailField(max_length=254, initial='',required=True,help_text='Optional.')
    date_of_birth = forms.DateField(widget=forms.SelectDateWidget(years=DOY),required=False,help_text="Optional.")
    gender = forms.ChoiceField(widget=forms.RadioSelect(), choices=(('male','male'),('female','female'),('other','other')),
                               initial="", required=False,  help_text='Optional.')
