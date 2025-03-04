<script lang="ts">
	import type { PageProps } from '../../../../.svelte-kit/types/src/routes';
	import Modal from '../Modal.svelte';
	import Footer from '$lib/components/footer/Footer.svelte';

	const MODO = {
		VISUALIZACAO: {
			valor: 'VISUALIZACAO',
			estilo: ''
		},
		EDICAO: {
			valor: 'EDICAO',
			estilo: 'border-7 border-orange-500'
		}
	};

	let { data }: PageProps = $props();

	let mode = $state(MODO.VISUALIZACAO.valor);
	let listaDiasSelecionadosEdicao: string[] = $state([]);
	let showModal = $state(false);

	let competencias = data?.calendario?.competencias;

	const formatDate = (date: string) => date.split('/')[0];

	/*
	* TODO: Autenticar usuário e pegar a role.
	* */
	const isAdmin = true;

	const handleClickEditarModoEdicao = () => {
		mode = mode === MODO.EDICAO.valor
			? MODO.VISUALIZACAO.valor
			: MODO.EDICAO.valor;
	};

	const handleClickDiaCalendario = (data: string): void => {
		if (!isAdmin) {
			return;
		}

		if (mode === MODO.VISUALIZACAO.valor) {
			return;
		}

		const id = data;

		if (!id) {
			alert('Could not get Element Id');
			return;
		}

		const idx = listaDiasSelecionadosEdicao.findIndex(d => d === id);
		if (idx === -1) {
			listaDiasSelecionadosEdicao.push(id);
			return;
		}

		listaDiasSelecionadosEdicao = listaDiasSelecionadosEdicao.filter(d => d !== id);
	};
</script>

<div class="prevent-select text-center min-h-[100%] {mode === MODO.EDICAO.valor && MODO.EDICAO.estilo}">
	<h1 class="display-4">Calendário Geral</h1>
	<p>MODO - <strong> {mode} </strong></p>

	{#if isAdmin}
		<button onclick={handleClickEditarModoEdicao}>Editar</button>
	{/if}

	<button onclick={() => (showModal = true)}> show modal</button>

	<Modal bind:showModal>
		{#snippet header()}
			<h2>
				modal
				<small><em>adjective</em> mod·al \ˈmō-dəl\</small>
			</h2>
		{/snippet}

		<div class="main-content">
			<div class="flex flex-col gap-5">
				<p>Dias selecionados</p>
				<div class="flex flex-col gap-3">
					{#each listaDiasSelecionadosEdicao as dia}
						<p>{dia}</p>
					{/each}
				</div>

				<input type="text" placeholder="Digite o nome do evento:" />

				<button>Salvar</button>
			</div>
		</div>
	</Modal>

	<div class="calendar-container">
		{#each competencias as competencia}
			<div class="b-0">
				<div class="calendar">
					<div class="month-indicator">{competencia?.mes}</div>
					<div class="day-of-week">
						<div>Dom</div>
						<div>Seg</div>
						<div>Ter</div>
						<div>Qua</div>
						<div>Qui</div>
						<div>Sex</div>
						<div>Sáb</div>
					</div>
					<div class="date-grid">
						{#each competencia?.dias as dia}
							{#if dia?.data == ""}
								<button class="border border-[#ddd]" aria-label="espaço em branco">
									<span>&nbsp</span>
								</button>
							{:else}
								<button id={dia?.data}
												class=" border-[#ddd]
												{listaDiasSelecionadosEdicao.find(x => x === dia?.data) !== undefined ?
												 'border border-red-400'
												  : 'border'}"
												onclick={() => handleClickDiaCalendario(dia?.data)}
								>
									<time datetime="{dia?.data}">{formatDate(dia?.data)}</time>
								</button>
							{/if}
						{/each}
					</div>
				</div>
			</div>
		{/each}
	</div>
	<Footer sessaoAtual="Geral" trilhas={data.trilhas} />
</div>

<style>
    * {
        cursor: default;
    }

    .calendar-container {
        display: flex;
        gap: 3em;
        flex-wrap: wrap;
    }

    .calendar {
        max-width: 400px;
        margin: 20px auto;
        padding: 15px;
        border-radius: 8px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        background-color: #ffffff;
    }

    .month-indicator {
        text-align: center;
        font-size: 1.5rem;
        font-weight: bold;
        margin-bottom: 10px;
        color: #333;
    }

    .day-of-week {
        display: grid;
        grid-template-columns: repeat(7, 1fr);
        text-align: center;
        font-weight: bold;
        color: #555;
        padding: 5px 0;
        background-color: #f8f9fa;
        border-radius: 6px 6px 0 0;
    }

    .date-grid {
        display: grid;
        grid-template-columns: repeat(7, 1fr);
        gap: 5px;
        padding: 10px;
        background-color: #fdfdfd;
        border-radius: 0 0 6px 6px;
    }

    button {
        width: 100%;
        padding: 10px;
        text-align: center;
        background-color: #ffffff;
        border-radius: 4px;
        transition: all 0.3s ease;
        font-size: 1rem;
    }

    button:hover {
        background-color: #007bff;
        color: white;
        border-color: #0056b3;
    }

    button:disabled {
        background: none;
        border: none;
        cursor: default;
    }

    button span {
        visibility: hidden;
    }
</style>
