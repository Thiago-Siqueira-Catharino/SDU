from django.db import models
from django.contrib.auth.models import AbstractUser


# Create your models here.
class Usuario(AbstractUser):
	class Role(models.TextChoices):
		CIDADAO = "CIDADAO", "Cidadao"
		PROFISSIONAL = "PROFISSIONAL", "Profissional"
		INSTITUICAO = "INSTITUICAO", "Instituicao"

	base_role = Role.CIDADAO
	role = models.CharField(max_length=50, choices=Role.choices, default=base_role)

	def save(self, *args, **kwargs):

		if self.pk:
			original = Usuario.objects.get(pk=self.pk)

			if original.role != self.role:
				self.role = original.role

		return super().save(*args, **kwargs)

class PerfilCidadao(models.Model):
	user = models.OneToOneField(Usuario, on_delete=models.CASCADE, related_name="perfil_cidadao")
	cpf = models.CharField(max_length=11)

class PerfilMedico(models.Model):
	user = models.OneToOneField(Usuario, on_delete=models.CASCADE, related_name="perfil_medico")
	crm = models.CharField(max_length=50)

class PerfilInstituicao(models.Model):
	user = models.OneToOneField(Usuario, on_delete=models.CASCADE, related_name="perfil_instituicao")
	cnpj = models.CharField(max_length=14)
