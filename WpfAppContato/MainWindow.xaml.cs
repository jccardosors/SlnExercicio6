using System;
using System.Collections.Generic;
using System.Windows;
using WpfAppContato.Utils;

namespace WpfAppContato
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool ordenacao = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            carregarDados();
        }

        private void carregarDados()
        {
            try
            {
                macDataGrid.ItemsSource = BancoAcao.Instance.RetornarlistaOrdenada(ordenacao);
                resetarBotoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        
        private void macDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.macDataGrid.SelectedCells.Count > 0)
            {
                var contato = (Contato)macDataGrid.SelectedItem;
                if (contato != null)
                {
                    this.txtContatoId.Text = contato.Id.ToString();
                    this.txtContatoNome.Text = contato.Nome;
                    this.txtContatoEmail.Text = contato.Email;
                    this.txtContatoTelefone.Text = contato.Telefone;

                    btnInserir.IsEnabled = false;
                    btnConsultar.IsEnabled = false;
                    btnAtualizar.IsEnabled = true;
                    btnDeletar.IsEnabled = true;
                    btnCancelar.IsEnabled = true;
                }
            }
        }

        #region Metodos Inclusao

        private void btnInserir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validarCampos())
                {
                    var contatoNovo = new Contato
                    {
                        Nome = txtContatoNome.Text,
                        Email = txtContatoEmail.Text,
                        Telefone = txtContatoTelefone.Text
                    };

                    this.inserirContato(contatoNovo);
                    this.carregarDados();
                    this.resetarBotoes();
                    MessageBox.Show("Registro adicionado com sucesso!");
                }
                else
                {
                    MessageBox.Show("Todo os campos são obrigatórios");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void inserirContato(Contato contato)
        {
            BancoAcao.Instance.IncluirItem(contato);
        }

        #endregion

        #region Metodos Edicao

        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validarCampos())
                {
                    var contatoNovo = new Contato
                    {
                        Id = Convert.ToInt32(txtContatoId.Text),
                        Nome = txtContatoNome.Text,
                        Email = txtContatoEmail.Text,
                        Telefone = txtContatoTelefone.Text
                    };

                    if (BancoAcao.Instance.AtualizarItem(contatoNovo))
                    {
                        this.carregarDados();
                        this.resetarBotoes();
                        MessageBox.Show("Registro adicionado com sucesso!");
                    }
                    else
                    {
                        MessageBox.Show("Falha ao tentar modificar o contato");
                    }                                       
                }
                else
                {
                    MessageBox.Show("Todo os campos são obrigatórios");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion

        #region Metodos Remocao

        private void btnDeletar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validarCampos())
                {
                    this.removerContato(txtContatoNome.Text);
                    this.carregarDados();
                    this.resetarBotoes();
                    MessageBox.Show("Registro removido com sucesso!");
                }
                else
                {
                    MessageBox.Show("Selecione um registro da grid para excluir");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void removerContato(String nome)
        {
            BancoAcao.Instance.RemoverItem(nome);
        }

        #endregion        

        #region Metodos Consulta

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtContatoNome.Text.Trim()))
                {
                    var item = this.pegarContatoPorNome(txtContatoNome.Text);
                    List<Contato> itemList = new List<Contato>();
                    itemList.Add(item);
                    macDataGrid.ItemsSource = itemList;

                    btnInserir.IsEnabled = false;
                    btnConsultar.IsEnabled = true;
                    btnAtualizar.IsEnabled = false;
                    btnDeletar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;
                    this.resetarBotoes();
                }
                else
                {
                    MessageBox.Show("Informe um valor no campo Nome para consultar1");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public Contato pegarContatoPorNome(String nome)
        {
            return BancoAcao.Instance.RetornarItemPorNome(nome);
        }

        private void btnOrdenar_Click(object sender, RoutedEventArgs e)
        {
            ordenacao = !ordenacao;
            if (ordenacao)
            {
                this.ordenarPorNome("ASC");
            }
            else
            {
                this.ordenarPorNome("DESC");
            }
        }

        public void ordenarPorNome(String ordemDeOrdenacao)
        {
            try
            {
                if (ordemDeOrdenacao.Trim().ToUpper().Equals("ASC"))
                {
                    macDataGrid.ItemsSource = BancoAcao.Instance.RetornarlistaOrdenada(true);
                }
                else
                {
                    macDataGrid.ItemsSource = BancoAcao.Instance.RetornarlistaOrdenada(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Metodos Impressao

        public void imprimir()
        {

        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Outros Metodos
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.resetarBotoes();
            this.carregarDados();
        }

        private bool validarCampos()
        {
            var retorno = true;
            if (string.IsNullOrEmpty(txtContatoNome.Text))
            {
                retorno = false;
            }
            else if (string.IsNullOrEmpty(txtContatoEmail.Text))
            {
                retorno = false;
            }
            else if (string.IsNullOrEmpty(txtContatoTelefone.Text))
            {
                retorno = false;
            }
            return retorno;
        }
        private void resetarBotoes()
        {
            btnInserir.IsEnabled = true;
            btnConsultar.IsEnabled = true;
            btnImprimir.IsEnabled = true;
            btnAtualizar.IsEnabled = false;
            btnDeletar.IsEnabled = false;
            btnOrdenar.IsEnabled = true;
            btnCancelar.IsEnabled = true;

            txtContatoId.Text = string.Empty;
            txtContatoNome.Text = string.Empty;
            txtContatoEmail.Text = string.Empty;
            txtContatoTelefone.Text = string.Empty;
        }

        #endregion
    }
}
