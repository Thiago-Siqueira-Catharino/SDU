from django.conf import settings
import boto3
import os

def uplaoad_s3(file_obj, file_name):
    s3 = boto3.client(
        's3',
        aws_access_key_id = settings.AWS_ACCESS_KEY_ID,
        aws_secret_access_key = settings.AWS_SECRET_ACCESS_KEY,
        region_name = settings.REGION_NAME
    )

    s3.upload_fileobj(file_obj, "nome_do_bucket",file_name)
    url = f"https://{settings.BUCKET_NAME}.s3.{settings.AWS_REGION}.amazonaws.com/{file_name}"
    return url