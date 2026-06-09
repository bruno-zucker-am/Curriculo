import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { environment } from '../../environments/environment';

// Importação do plugin de câmera do Capacitor
import { Camera, MediaTypeSelection } from '@capacitor/camera';
import { Capacitor } from '@capacitor/core';

import {
  IonContent,
  IonHeader,
  IonTitle,
  IonToolbar,
  IonButton,
  IonInput,
  IonItem,
  IonLabel,
  IonTextarea,
  IonImg,
} from '@ionic/angular/standalone';

@Component({
  selector: 'app-curriculo',
  templateUrl: './curriculo.html',
  styleUrls: ['./curriculo.scss'],
  standalone: true,
  imports: [
    IonContent,
    IonHeader,
    IonTitle,
    IonToolbar,
    IonButton,
    IonInput,
    IonItem,
    IonLabel,
    IonTextarea,
    IonImg,
    CommonModule,
    FormsModule,
  ],
})
export class Curriculo implements OnInit {
  // URL do backend para salvar o currículo
  private apiUrl = environment.apiUrl + 'Curriculo';

  // Injeção do HttpClient para comunicação com o backend
  constructor(private http: HttpClient) {}

  // Método de inicialização do componente
  ngOnInit() {}

  // Estrutura do currículo com informações pessoais, endereço, formações, cursos e experiências
  curriculo = {
    informacao: {
      foto: '',
      nome: '',
      idade: null,
      telefone: '',
      relacionamento: '',
      email: '',
      objetivo: '',
      perfil: '',
    },
    endereco: {
      rua: '',
      numero: '',
      bairro: '',
      cidade: '',
      estado: '',
      cep: '',
    },
    formacoes: [{ curso: '', instituicao: '', anoConclusao: null }],
    cursos: [{ nomeCurso: '', instituicaoCurso: '', anoConclusaoCurso: null }],
    experiencias: [
      { empresa: '', cargo: '', anoInicio: null, anoFim: null, atividades: '' },
    ],
  };

  // Função para adicionar nova formação
  adicionarFormacao() {
    this.curriculo.formacoes.push({
      curso: '',
      instituicao: '',
      anoConclusao: null,
    });
  }

  // Função para adicionar novo curso
  adicionarCurso() {
    this.curriculo.cursos.push({
      nomeCurso: '',
      instituicaoCurso: '',
      anoConclusaoCurso: null,
    });
  }

  // Função para adicionar nova experiência
  adicionarExperiencia() {
    this.curriculo.experiencias.push({
      empresa: '',
      cargo: '',
      anoInicio: null,
      anoFim: null,
      atividades: '',
    });
  }

  // Função para selecionar uma foto da galeria e salvar no currículo
  async adicionarFoto() {
    try {
      const { results } = await Camera.chooseFromGallery({
        quality: 60,
        // Permitir edição in-app
        editable: 'in-app',
        // Redimensionamento
        targetWidth: 600,
        targetHeight: 800,
        mediaType: MediaTypeSelection.Photo,
        includeMetadata: true,
      });

      const imageResult = results && results.length ? results[0] : null;
      if (!imageResult) return;

      // Obtém o caminho da foto: webPath para web, uri para mobile
      const caminho =
        imageResult.webPath ??
        (imageResult.uri ? Capacitor.convertFileSrc(imageResult.uri) : '');

      if (!caminho) {
        alert('Não foi possível carregar a foto selecionada.');
        return;
      }

      try {
        const base64Foto = await this.converterImagem(caminho);
        this.curriculo.informacao.foto = base64Foto;

        console.log(
          'Tamanho da foto otimizada:',
          (base64Foto.length / 1024).toFixed(2),
          'KB',
        );
      } catch (erroConversao) {
        console.error('Erro ao converter imagem:', erroConversao);
        alert('Erro ao processar a foto selecionada.');
      }
    } catch (e) {
      // Se o usuário cancelar, não exibe erro
      if ((e as any).message !== 'User cancelled photos app') {
        console.error('Falha ao selecionar da galeria:', e);
        alert('Erro ao selecionar a foto da galeria.');
      }
    }
  }

  // Função para converter, recortar quadrado (center crop) e padronizar a imagem
  private async converterImagem(caminho: string): Promise<string> {
    const response = await fetch(caminho);
    const blob = await response.blob();

    return new Promise<string>((resolve, reject) => {
      const imagemOrigem = new Image();
      const urlObjeto = URL.createObjectURL(blob);

      imagemOrigem.onload = () => {
        const canvas = document.createElement('canvas');
        const contexto = canvas.getContext('2d');

        if (!contexto) {
          URL.revokeObjectURL(urlObjeto);
          reject('Não foi possível obter o contexto do canvas.');
          return;
        }

        // Faz o quadrado da foto
        const ladoMenor = Math.min(imagemOrigem.width, imagemOrigem.height);

        // 2. Calcula onde o recorte deve começar para ficar bem no centro da foto
        const offsetX = (imagemOrigem.width - ladoMenor) / 2;
        const offsetY = (imagemOrigem.height - ladoMenor) / 2;

        // Define o tamanho final máximo da imagem gerada (ex: 600x600 px)
        const tamanhoFinal = Math.min(ladoMenor, 600);

        // Configura o canvas para ser quadrado
        canvas.width = tamanhoFinal;
        canvas.height = tamanhoFinal;

        // Fundo branco para evitar o fundo preto de PNGs transparentes
        contexto.fillStyle = '#FFFFFF';
        contexto.fillRect(0, 0, canvas.width, canvas.height);

        // Desenha a imagem usando o recorte central
        // Os 4 primeiros parâmetros depois da imagem são a área de recorte.
        // Os 4 últimos são onde ela será colada no canvas.
        contexto.drawImage(
          imagemOrigem,
          offsetX,
          offsetY,
          ladoMenor,
          ladoMenor, // Recortando o centro da original
          0,
          0,
          tamanhoFinal,
          tamanhoFinal, // Colando no canvas redimensionada
        );

        // Exporta para JPEG com qualidade 80%
        const base64Padronizado = canvas.toDataURL('image/jpeg', 0.8);

        URL.revokeObjectURL(urlObjeto);
        resolve(base64Padronizado);
      };

      // Prevenção de erro caso a imagem falhe ao carregar
      imagemOrigem.onerror = () => {
        URL.revokeObjectURL(urlObjeto);
        reject('Erro ao processar a imagem.');
      };

      // Inicia o carregamento da imagem
      imagemOrigem.src = urlObjeto;
    });
  }

  // Função para enviar o currículo para o backend e iniciar o download do PDF gerado
  salvarPdf() {
    this.http
      .post(this.apiUrl, this.curriculo, {
        responseType: 'blob',
        observe: 'response',
      })
      .subscribe({
        next: (response: HttpResponse<Blob>) => {
          const blob: Blob | null = response.body;

          if (!blob) {
            alert('Não foi possível obter o PDF do servidor.');
            return;
          }

          let fileName = `Curriculo_${this.curriculo.informacao.nome.replace(' ', '_')}.pdf`;
          const contentDisposition = response.headers.get(
            'Content-Disposition',
          );

          if (contentDisposition) {
            const match = contentDisposition.match(/filename="([^"]+)"/);
            if (match && match[1]) {
              fileName = match[1];
            }
          }

          // Chama a função de download para gerar o arquivo PDF no navegador
          this.downloadPdfWeb(blob, fileName);
          alert('Download iniciado!');
        },
        error: (_erro) => {
          console.error('Erro ao salvar o currículo:', _erro);
          alert('Erro ao salvar o currículo. Verifique o console.');
        },
      });
  }

  // Gera o download do PDF no navegador usando a API de Blob e URL
  downloadPdfWeb(blob: Blob, fileName: string): void {
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');

    link.href = url;

    // Define o nome do arquivo para download dinamicamente com base no nome do usuário
    link.download = fileName;

    document.body.appendChild(link);
    link.click();

    // Limpeza
    document.body.removeChild(link);
    setTimeout(() => window.URL.revokeObjectURL(url), 100);
  }
}
