from pprint import pprint
from django.contrib.auth.decorators import login_required
from django.http import HttpResponse
from django.shortcuts import render, redirect
from rest_framework.renderers import TemplateHTMLRenderer
from rest_framework.response import Response
from rest_framework.views import APIView
from post_app.forms import *
from post_app.serializers import *
from . models import Text
from django.core.exceptions import ObjectDoesNotExist

#region Class-Based Views

class LinkView(APIView):
    def get(self, request):
        form = LinkForm()
        return render(request, 'post_app/create_link.html', {'form': form})

    def post(self, request):
        data = request.data
        user = request.user
        title = data.get('title')
        link = data.get('link')
        nsfw = False if data.get('nsfw') is None else data.get('nsfw')
        original_content = False if data.get('original_content') is None else data.get('original_content')
        spoiler = False if data.get('spoiler') is None else data.get('spoiler')

        serializer = LinkSerializer(
            data={'user': user.id, 'title': title, 'url': link, 'nsfw': nsfw, 'original_content': original_content,
                  'spoiler': spoiler})

        serializer.is_valid()

        if not serializer.is_valid():
            form = LinkForm(request.data)
            return render(request, 'post_app/create_link.html', {'form': form})

        serializer.save()
        return redirect('/')


class TextView(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'post_app/create_text.html'
    style = {'template_pack': 'rest_framework/vertical/'}

    def get(self, request):
        form = TextForm()
        return render(request, 'post_app/create_text.html', {'form': form})


    def post(self, request):
        data = request.data
        user = request.user
        title = data.get('title')
        text = data.get('text')
        nsfw = False if data.get('nsfw') is None else data.get('nsfw')
        original_content = False if data.get('original_content') is None else data.get('original_content')
        spoiler = False if data.get('spoiler') is None else data.get('spoiler')


        serializer = TextSerializer(data={'user': user.id,
                                          'title':title ,
                                          'text':text,
                                          'nsfw': nsfw,
                                          'original_content' : original_content,
                                          'spoiler':spoiler})

        if not serializer.is_valid():
            form = TextForm(request.data)
            return render(request, 'post_app/create_text.html', {'form': form})

        serializer.save()
        return redirect('/')


class FileView(APIView):
    renderer_classes = [TemplateHTMLRenderer]
    template_name = 'post_app/create_file.html'
    style = {'template_pack': 'rest_framework/vertical/'}

    def get(self, request):
        form = FileForm(request.FILES)
        return render(request, 'post_app/create_file.html', {'form': form})


    def post(self, request):
        data = request.data
        user = request.user
        title = data.get('title')
        file = data.get('file')
        nsfw = False if data.get('nsfw') is None else data.get('nsfw')
        original_content = False if data.get('original_content') is None else data.get('original_content')
        spoiler = False if data.get('spoiler') is None else data.get('spoiler')


        serializer = FileSerializer(data={'user': user.id,
                                          'title':title ,
                                          'file':file,
                                          'nsfw': nsfw,
                                          'original_content' : original_content,
                                          'spoiler':spoiler})

        if not serializer.is_valid():
            form = FileForm(request.data,request.FILES)
            return render(request, 'post_app/create_file.html', {'form': form})

        serializer.save()
        return redirect('/')


class CommentView(APIView):


    def post(self, request, post_id, type=''):
        data = request.data

        if (type == ''):
            type = request.path.split('/')[2]

        dat = {'user':request.user.id,'created_date':datetime.now(), 'comment' : request.data.get('comment'),
               'content_type':ContentType.objects.get(model=type.strip('s')).id, 'object_id':post_id }
        serializer = CommentSerializer(data= dat )




        if not serializer.is_valid():
            print(serializer.errors)
            return redirect("/post/"+type+"/"+post_id)



        serializer.save()
        return redirect("/post/"+type+"/"+post_id)


class TextList(APIView):

    renderer_classes = [TemplateHTMLRenderer]
    template_name = "post_app/home.html"
    def get(self, request):
        texts = Text.objects.all()
        serializer = TextSerializer(texts, many=True)
        pprint({'texts': serializer.data})
        return Response({'texts': serializer.data})

#endregion

#region Function-Based Views
@login_required(login_url='/accounts/login/')
def my_posts(request):
    posts = []
    texts, files, links =[],[],[]
    try:
        texts = Text.objects.filter(user=request.user.id)
        files = File.objects.filter(user=request.user.id)
        links = Link.objects.filter(user=request.user.id)
    except ObjectDoesNotExist:
        pass
    posts.extend(texts)
    posts.extend(files)
    posts.extend(links)

    return render(request, 'post_app/all_posts.html', {'elems':posts})

@login_required(login_url='/accounts/login/')
def texts(request):
    texts = Text.objects.all()

    template = 'post_app/texts.html'
    context = {'texts': texts}
    return render(request, template, context)


@login_required(login_url='/accounts/login/')
def links(request):
    links = Link.objects.all()

    template = 'post_app/links.html'
    context = {'links': links}
    return render(request, template, context)


@login_required(login_url='/accounts/login/')
def files(request):
    files = File.objects.all()
    template = 'post_app/files.html'
    context = {'files': files}
    return render(request, template, context)

@login_required(login_url='/accounts/login/')
def all_posts(request):
    posts = Post.objects.all()
    context = {'posts':posts}
    return render(request, 'all_posts', context)

@login_required(login_url='/accounts/login/')
def create_post(request):
    if request.method == "POST":
        value = request.POST.get('post')

        if value == 'text':
            return TextView().get(request)
        elif value == 'file':
            return FileView().get(request)
        else:
            return LinkView().get(request)


    else:
        context = {}
        return render(request,'post_app/create_post.html', context)



def get_post_of_type(type, post_id):

    if type == "texts":
        post = Text.objects.get(id=post_id)
    elif type == "files":
        post = File.objects.get(id=post_id)
    elif type == "links":
        post = Link.objects.get(id = post_id)
    else:
        raise Exception("There are three types")
    return post



@login_required(login_url='/accounts/login/')
def like(request, post_id, type):

    p = get_post_of_type(type, post_id)
    '''
    content_type = models.ForeignKey(ContentType, on_delete=CASCADE)
    object_id = models.PositiveIntegerField()
    post = GenericForeignKey()

    '''
    try:
        Like.objects.get(user=request.user, object_id = p.id, content_type=ContentType.objects.get(model=type.strip('s')).id)
        return render(request, 'post_app/post.html', {'liked': True, 'post':p, 'post_id':p.id, 'type':type})
    except ObjectDoesNotExist:
        # cool, user hasn't liked this post yet
        Like.objects.create(user=request.user, object_id=p.id,content_type=ContentType.objects.get(model=type.strip('s')), created_date= datetime.now())
        p.nr_likes += 1
        p.save()

    return redirect('/post/'+type+'/'+post_id)



@login_required(login_url='/accounts/login/')
def post(request, post_id, type=''):
    template_name = 'post_app/post.html'
    if type == '':
        type = request.path.split('/')[2]
    post = get_post_of_type(type, post_id)


    comments = post.comments.all()
    comment_form = CommentForm()
    context = {'type': type, 'post_id': post_id, 'post': post ,'comments': comments, 'comment_form':comment_form  }

    try:
        Like.objects.get(user=request.user, object_id=post.id,
                         content_type=ContentType.objects.get(model=type.strip('s')).id)
        context['liked'] = True

    except ObjectDoesNotExist:
        pass

    return render(request, template_name, context)

#endregion