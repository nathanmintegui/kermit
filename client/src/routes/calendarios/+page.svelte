<script lang="ts">
	import Footer from '$lib/components/footer/Footer.svelte';
	import Modal from './components/Modal.svelte';
	import type { PageProps } from './$types';
	import { page } from '$app/state';

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
	const isAdmin = false;

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

	const getNomeTrilha = (): string => {
		const param = page.url.searchParams.get('trilha');

		const nomeTrilha = data.trilhas.find(d => d.calendarioId === param);

		return nomeTrilha !== null ? nomeTrilha?.trilha : 'Geral';
	};
</script>

<div id="page"
		 class="custom-cursor prevent-select text-center min-h-[100%] {mode === MODO.EDICAO.valor && MODO.EDICAO.estilo}">
	<header class="border-b p-7 bg-white">
		<h1 class="font-bold text-3xl">Calendário {getNomeTrilha()}</h1>
	</header>

	<div class="page-container">
		{#if isAdmin}
			<button onclick={handleClickEditarModoEdicao}>Editar</button>
			<button onclick={() => (showModal = true)}> show modal</button>
		{/if}

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

					<form method="POST" action="?/addEvent">
						<input type="text" placeholder="Digite o nome do evento:" name="evento" />

						<input name="dias" type="hidden" value={JSON.stringify(listaDiasSelecionadosEdicao)}>

						<button>Salvar</button>
					</form>
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
								{#if dia?.data === ""}
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
	</div>

	<Footer sessaoAtual={getNomeTrilha()} trilhas={data.trilhas} />
</div>

<style>
    * {
        font-family: "Poppins", sans-serif;
        cursor: url("data:image/svg+xml,<svg xmlns='http://www.w3.org/2000/svg' width='24' height='48' viewBox='0 0 24 24'><path fill='%23FFF' stroke='%23000' stroke-width='2' stroke-linejoin='round' d='M10 11V8.99c0-.88.59-1.64 1.44-1.86h.05A1.99 1.99 0 0 1 14 9.05V12v-2c0-.88.6-1.65 1.46-1.87h.05A1.98 1.98 0 0 1 18 10.06V13v-1.94a2 2 0 0 1 1.51-1.94h0A2 2 0 0 1 22 11.06V14c0 .6-.08 1.27-.21 1.97a7.96 7.96 0 0 1-7.55 6.48 54.98 54.98 0 0 1-4.48 0 7.96 7.96 0 0 1-7.55-6.48C2.08 15.27 2 14.59 2 14v-1.49c0-1.11.9-2.01 2.01-2.01h0a2 2 0 0 1 2.01 2.03l-.01.97v-10c0-1.1.9-2 2-2h0a2 2 0 0 1 2 2V11Z'></path></svg>") 12 24, auto;
    }

    #page {
        background-color: rgba(255, 255, 255, 1);
        background-image: radial-gradient(rgba(0, 0, 0, 0.2) 0.9px, rgba(255, 255, 255, 1) 0.9px);
        background-size: 18px 18px;
    }

    .page-container {
        display: flex;
        flex-direction: column;
        padding-top: 6em;
    }

    .calendar-container {
        display: flex;
        justify-content: space-evenly;
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
