from django.shortcuts import render

# Create your views here.
def render_front(request):
    return render(request, 'index.html')