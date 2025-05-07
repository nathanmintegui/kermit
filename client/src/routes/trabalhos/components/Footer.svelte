<script lang="ts">
	const secoes = ['Trabalhos', 'Calendário'];
</script>

<div class="absolute bottom-0 flex w-screen gap-5 border-t">
	<div class="flex gap-5 p-1">
		{#each secoes as secao}
			<p class="pr-3 hover:cursor-pointer hover:font-bold">{secao}</p>
		{/each}
	</div>
</div>
